using BookAPI.Data;
using BookAPI.Repositories;
using BookAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IRepository<Book, int> _bookRepository;
        private readonly IRepository<Author, int> _authorRepository;
        private readonly IRepository<Publisher, int> _publisherRepository;
        public PublishersController(IRepository<Book, int> bookRepository,
            IRepository<Author, int> authorRepository
            , IRepository<Publisher, int> publisherRepository)

        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<PublisherResource>> GetPublishers(string author)
        {
            var entities = await _publisherRepository.Get();

            var listOfPublisherResource = new List<PublisherResource>();
            var listOfBookResource = new List<BookResource>();


            foreach (var item in entities)
            {
                foreach (var book in item.Books)
                {
                    var bookResource = new BookResource
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = book.Description,
                        IsAvailable = book.IsAvailable,
                        Publisher = book.Publisher?.Name,
                    };

                    listOfBookResource.Add(bookResource);
                }
                var publisherResource = new PublisherResource
                {
                    Name = item.Name,
                    Books = listOfBookResource
                };

                listOfPublisherResource.Add(publisherResource);
            }
            Console.WriteLine(listOfPublisherResource);
            return listOfPublisherResource;

        }



        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherResource>> GetPublishers(int id)
        {
            var publisherFromRepo = await _publisherRepository?.Get(id);
            if (publisherFromRepo == null)
            {
                return NotFound();
            }
            var publisherResource = new PublisherResource
            {
                Name = publisherFromRepo.Name,
            };

            return Ok(publisherResource);
        }


        [HttpPost]
        public async Task<ActionResult<PublisherResource>> PostPubliser([FromBody] PublisherModel publisherModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Here map (model) -> entity
            var publisherEntity = new Publisher
            {
                Name = publisherModel.Name,
            };
            // Entity from Book
            var newPublisher = await _publisherRepository.Create(publisherEntity);

            // Here map (newBook which is Entity) -> Resource
            var publisherResource = new PublisherResource
            {
                Name = newPublisher.Name,
            };


            return CreatedAtAction(nameof(GetPublishers), new { id = newPublisher.Id }, publisherResource);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PublisherResource>> PutPublihser(int id, [FromBody] PublisherModel publisherModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // You can make it like Yazan said from his document.
            var publisherToUpdate = await _publisherRepository?.Get(id);

            publisherToUpdate.Name = publisherModel.Name;


            if (publisherModel == null)
                return NotFound();
            await _publisherRepository.Update(publisherToUpdate);
            var publisherResource = new PublisherResource
            {
                Name = publisherToUpdate.Name,
            };
            //JObject obj = (JObject)JToken.FromObject(bookResource);
            //Console.WriteLine(bookResource);
            return Ok(publisherResource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var publisherToDelete = await _publisherRepository.Get(id);
            if (publisherToDelete == null)
                return NotFound();

            await _publisherRepository.Delete(publisherToDelete.Id);
            return NoContent();
        }


    }
}
