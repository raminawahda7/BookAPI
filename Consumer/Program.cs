using BookAPI.Data;
using BookAPI.Repositories;
using BookAPI.Repositories.Interfaces;
using Consumer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            Configure(collection);
            var services = collection.BuildServiceProvider();
            //var _repo = service.GetRequiredService<IRepository<Author, int>>();
            var _authorService = services.GetRequiredService<IAuthorPublisherServices>();
            // ---------------------------------------

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "author", durable: true, exclusive: false, autoDelete: false, arguments: null);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                    var authorObj = JsonConvert.DeserializeObject<toReceive>(content);
                    switch (authorObj.Type)
                    {
                        case "create":
                            await _authorService.CreateAuthor(authorObj.Id);
                            break;
                        //case "update":
                        //    var authorToUpdate = await _repo.Get(authorObj.Id);
                        //    await _repo.Update(authorToUpdate);
                        //    break;
                        //case "delete":
                        //    await _repo.Delete(authorObj.Id);
                        //    break;
                        default:
                            break;
                    }


                    Console.WriteLine($" [author] Received {0}  {authorObj.Type} consumed ...");
                };
                channel.BasicConsume(queue: "author", autoAck: true, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
        }
        public static void Configure(IServiceCollection services)
        {
            string connectionSql = "server=APPIATECH-RNAW;database=BookService;Trusted_Connection=true;";

            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionSql, b => b.MigrationsAssembly("BookAPI").UseNetTopologySuite()))
                .AddScoped<IRepository<Author, int>, SQLAuthorRepository>()
                .AddScoped<IAuthorPublisherServices, AuthorPublisherServices>()
                .AddHttpClient<IAuthorPublisherServices, AuthorPublisherServices>();
        }
    }
}
