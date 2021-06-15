using BookAPI.Data;
using BookAPI.Repositories;
using BookAPI.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
{
    class Program
    {

        static void Main(string[] args)
        {
            // ---------------------------------------

            var collection = new ServiceCollection();
            collection.AddDbContext<AppDbContext>()
                .AddScoped<IRepository<Author, int>, SQLAuthorRepository>();

            var service = collection.BuildServiceProvider();
            var _repo = service.GetRequiredService<IRepository<Author, int>>();

            // ---------------------------------------

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "author", durable: true, exclusive: false, autoDelete: false, arguments: null);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                    
                    var createdAuthor = JsonConvert.DeserializeObject<Author>(content);
                   var author =  _repo.Create(createdAuthor);

                    Console.WriteLine(" [author] Received {0}", createdAuthor.ToString());
                };
                channel.BasicConsume(queue: "author", autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }
    }
}
