using BookAPI.Data;
using Consumer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarvestController : ControllerBase
    {
        //private readonly IAuthorPublisherServices _authorPublisherServices;

        //public HarvestController(IAuthorPublisherServices authorPublisherServices)
        //{
        //    _authorPublisherServices = authorPublisherServices;
        //}
        //[HttpGet]
        //public async Task<IEnumerable<Publisher>> SyncData() 
        //{
        //   var data = await _authorPublisherServices.HarvestPublisher();
        //    return data;
        //}
        [HttpGet]
        public void SyncData()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "harvester",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string message = "hravestpublisher";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "harvester",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
