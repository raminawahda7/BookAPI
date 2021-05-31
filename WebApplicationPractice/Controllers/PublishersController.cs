using BookAPI.Data;
using BookAPI.Helper;
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
        public async Task<IEnumerable<PublisherResource>> GetPublishers(string publisher)
        {
            var entities = await _publisherRepository.Get();

            return entities.PublisherBookResource();
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherResource>> GetPublishers(int id)
        {
            var publisherFromRepo = await _publisherRepository.Get(id);
            if (publisherFromRepo == null)
            {
                return NotFound();
            }

            var resource = new List<PublisherBookResource>();
            foreach (var book in publisherFromRepo.Books)
            {
                resource.Add(new PublisherBookResource
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsAvailable = book.IsAvailable,
                    //AuthorNames = book.Authors.Select(e => e.FullName).ToList()
                });
            }
            var publisherResource = new PublisherResource
            {
                Id = publisherFromRepo.Id,
                Name = publisherFromRepo.Name,
                Books = resource
            };

            return Ok(publisherResource);
        }


        [HttpPost]
        public async Task<ActionResult<PublisherCreateResource>> PostPubliser([FromBody] PublisherModel publisherModel)
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
            var publisherResource = new PublisherCreateResource
            {
                Id=newPublisher.Id,
                Name = newPublisher.Name,
            };


            return CreatedAtAction(nameof(GetPublishers), new { id = newPublisher.Id }, publisherResource);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PublisherCreateResource>> PutPublihser(int id, [FromBody] PublisherModel publisherModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // You can make it like Yazan said from his document.
            var publisherToUpdate = await _publisherRepository.Get(id);

            if (publisherToUpdate == null)
                return NotFound();

            publisherToUpdate.Name = publisherModel.Name;

            await _publisherRepository.Update(publisherToUpdate);
            var publisherResource = new PublisherCreateResource
            {
                Id = publisherToUpdate.Id,
                Name = publisherToUpdate.Name,
            };
            return Ok(publisherResource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var publisherToDelete = await _publisherRepository.Get(id);

            if (publisherToDelete == null)
                return NotFound();
            if (publisherToDelete.Books.Count==0)
            {
            await _publisherRepository.Delete(publisherToDelete.Id);
            return NoContent();
            }
            return BadRequest("Can't delete publisher has book");
        }


    }
}
