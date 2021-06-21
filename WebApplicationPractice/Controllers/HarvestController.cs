using BookAPI.Data;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarvestController : ControllerBase
    {
        private readonly IRepository<Publisher, int> _publisherRepository;
        private readonly IRepository<Author, int> _authorRepository;

        public HarvestController(IRepository<Publisher, int> publisherRepository,IRepository<Author, int> authorRepository)
        {
            _publisherRepository = publisherRepository;
            _authorRepository = authorRepository;
        }
        [HttpGet]
        [Route("[controller]/publishers")]
        public async Task<IEnumerable<Publisher>>GetPublishers()
        {
            var publishers = await _publisherRepository.Get();

            return publishers;
        }
        [HttpGet]
        [Route("[controller]/authors")]
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            var authors = await _authorRepository.Get();

            return authors;
        }
        //public void Harvest()
        //{
        //    var factory = new ConnectionFactory() { HostName = "localhost" };
        //    using (var connection = factory.CreateConnection())
        //    using (var channel = connection.CreateModel())
        //    {
        //        channel.QueueDeclare(queue: "Harvest",
        //                             durable: false,
        //                             exclusive: false,
        //                             autoDelete: false,
        //                             arguments: null);

        //        channel.BasicPublish(exchange: "",
        //                             routingKey: "Harvest",
        //                             basicProperties: null,
        //                             body: new byte[1]);
        //    }
        //}
    }
}
