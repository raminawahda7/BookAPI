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
    public class AuthorsController : ControllerBase
    {
        private readonly IRepository<Book, int> _bookRepository;
        private readonly IRepository<Author, int> _authorRepository;
        private readonly IRepository<Publisher, int> _publisherRepository;
        public AuthorsController(IRepository<Book, int> bookRepository,
            IRepository<Author, int> authorRepository
            , IRepository<Publisher, int> publisherRepository)

        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<AuthorResource>> GetBooks(string author)
        {
            var entities = await _authorRepository.Get();
            //if(author != null)
            //    entities = entities.Where(e => e.Author.Contains(author));
            var listOfBookResource = new List<AuthorResource>();

            foreach (var item in entities)
            {
                var bookResource = new AuthorResource { 
                Id = item.Id,
                Title = item.Title,
                };

                listOfBookResource.Add(bookResource);
            }
            Console.WriteLine(listOfBookResource);
            return listOfBookResource;

        }



        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResource>> GetBooks(int id)
        {
            var bookFromRepo = await _authorRepository?.Get(id);
            if (bookFromRepo == null)
            {
                return NotFound();
            }
            var bookResource = new AuthorResource
            {
                Id = bookFromRepo.Id,
                Title = bookFromRepo.Title,
            };

            return Ok(bookResource);
        }


        [HttpPost]
        public async Task<ActionResult<AuthorResource>> PostBooks([FromBody] AuthorModel bookModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Here map (model) -> entity
            var bookEntity = new Book
            {
                Title = bookModel.Title,
                Description = bookModel.Description
            };
            // Entity from Book
            var newBook = await _authorRepository.Create(bookEntity);

            // Here map (newBook which is Entity) -> Resource
            var bookResource = new AuthorResource
            {
                Id = newBook.Id,
                Title = newBook.Title,
            };


            return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, bookResource);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorResource>> PutBooks(int id, [FromBody] AuthorModel bookModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // You can make it like Yazan said from his document.
            var bookToUpdate = await _authorRepository?.Get(id);

            bookToUpdate.Title = bookModel.Title;
            bookToUpdate.Description = bookModel.Description;


            if (bookToUpdate == null)
                return NotFound();
            await _authorRepository.Update(bookToUpdate);
            var bookResource = new AuthorResource
            {
                Id = bookToUpdate.Id,
                Title = bookToUpdate.Title,
            };
            JObject obj = (JObject)JToken.FromObject(bookResource);
            Console.WriteLine(bookResource);
            return Ok(bookResource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var bookToDelete = await _authorRepository.Get(id);
            if (bookToDelete == null)
                return NotFound();

            await _authorRepository.Delete(bookToDelete.Id);
            return NoContent();
        }
        
        
    }
}
