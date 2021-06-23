using BookAPI.Data;
using BookAPI.Helper;
using BookAPI.Repositories;
using Domain.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        public readonly IPublisherManager _publisherManager;
        //private readonly ILogger<PublishersController> _logger;

        public PublishersController(IPublisherManager publisherManager)

        {
            _publisherManager = publisherManager;
            //_logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _publisherManager.GetPublishers();

            return Ok(publishers);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublishers(int id)
        {
            var publisher = await _publisherManager.GetPublisher(id);

            return Ok(publisher);
        }


        [HttpPost]
        public async Task<IActionResult> PostPubliser([FromBody] PublisherModel publisherModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = await _publisherManager.PostPubliser(publisherModel);


            return Ok(publisher);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublihser(int id, [FromBody] PublisherModel publisherModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var publisher = await _publisherManager.PutPublihser(id, publisherModel);
            return Ok(publisher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var publisherToDelete = await _publisherManager.GetPublisher(id);

            if (publisherToDelete == null)
                return NoContent();
            if (publisherToDelete.Books.Count == 0)
            {
                await _publisherManager.DeletePublisher(publisherToDelete.Id);
                return NoContent();
            }
            else
                return BadRequest("Can't delete publisher has book");

        }


    }
}
