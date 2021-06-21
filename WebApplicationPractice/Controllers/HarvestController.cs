using BookAPI.Data;
using Consumer.Services;
using Domain.enums;
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
        [HttpGet]
        public void SyncData(ListOfHarvest listOfHarvest)
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

                var body = Encoding.UTF8.GetBytes(listOfHarvest.ToString());

                channel.BasicPublish(exchange: "",
                                     routingKey: "harvester",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
