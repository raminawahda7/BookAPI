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
            var _consumerService = services.GetRequiredService<IAuthorPublisherServices>();
            // ---------------------------------------

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                #region authorPublisher_Queue
                channel.QueueDeclare(queue: "authorPublisher", durable: true, exclusive: false, autoDelete: false, arguments: null);

                Console.WriteLine(" [*] Waiting for author messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                    var received = JsonConvert.DeserializeObject<toReceive>(content);
                    if (received.entityType == "author")
                    {
                        switch (received.ProcessType)
                        {
                            case "create":
                                await _consumerService.CreateAuthor(received.Id);
                                break;
                            case "update":
                                await _consumerService.UpdateAuthor(received.Id);
                                break;
                            case "delete":
                                await _consumerService.DeleteAuthor(received.Id);
                                break;
                            default:
                                break;
                        }
                        //Console.WriteLine($" [{received.entityType}] Received {0}  {received.ProcessType} consumed ...");
                    }
                    else if (received.entityType == "publisher")
                    {
                        switch (received.ProcessType)
                        {
                            case "create":
                                await _consumerService.CreatePublisher(received.Id);
                                break;
                            case "update":
                                await _consumerService.UpdatePublisher(received.Id);
                                break;
                            case "delete":
                                await _consumerService.DeletePublisher(received.Id);
                                break;
                            default:
                                break;
                        }
                        //Console.WriteLine($" [{received.entityType}] Received {0} and {received.ProcessType} consumed ...");
                    }
                    else
                    {
                        throw new Exception(" There is no type of entity in the message ....");
                    }
                };

                channel.BasicConsume(queue: "authorPublisher", autoAck: true, consumer: consumer);

                //Console.WriteLine("--------------------------------------------------");


                //Console.WriteLine(" Press [enter] to exit.");
                //Console.ReadLine();
                #endregion
                #region harvester_Queue
                channel.QueueDeclare(queue: "harvester",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                var harvest = new EventingBasicConsumer(channel);
                harvest.Received += async (model, ea) =>
                {
                    await _consumerService.Harvest();
                };
                channel.BasicConsume(queue: "harvester",
                                     autoAck: true,
                                     consumer: harvest);
                Console.ReadKey();

                #endregion
            }
        }
        public static void Configure(IServiceCollection services)
        {
            string connectionSql = "server=APPIATECH-RNAW;database=BookService;Trusted_Connection=true;";

            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionSql, b => b.MigrationsAssembly("BookAPI").UseNetTopologySuite()))
                .AddScoped<IRepository<Author, int>, SQLAuthorRepository>()
                .AddScoped<IRepository<Publisher, int>, SQLPublisherRepository>()
                .AddScoped<IAuthorPublisherServices, AuthorPublisherServices>()
                .AddHttpClient<IAuthorPublisherServices, AuthorPublisherServices>();
        }
    }
}
