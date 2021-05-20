using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<BookResource>> GetBooks(string author)
        {
            var entities = await _bookRepository.Get();
            if(author != null)
                entities = entities.Where(e => e.Author.Contains(author));
            var listOfBookResource = new List<BookResource>();

            foreach (var item in entities)
            {
                var bookResource = new BookResource { 
                Id = item.Id,
                Title = item.Title,
                Author = item.Author
                };

                listOfBookResource.Add(bookResource);
            }
            Console.WriteLine(listOfBookResource);
            return listOfBookResource;

        }



        [HttpGet("{id}")]
        public async Task<ActionResult<BookResource>> GetBooks(int id)
        {
            var bookFromRepo = await _bookRepository?.Get(id);
            if (bookFromRepo == null)
            {
                return NotFound();
            }
            var bookResource = new BookResource
            {
                Id = bookFromRepo.Id,
                Title = bookFromRepo.Title,
                Author = bookFromRepo.Author
            };

            return Ok(bookResource);
        }


        [HttpPost]
        public async Task<ActionResult<BookResource>> PostBooks([FromBody] BookModel bookModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Here map (model) -> entity
            var bookEntity = new Book
            {
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description
            };
            // Entity from Book
            var newBook = await _bookRepository.Create(bookEntity);

            // Here map (newBook which is Entity) -> Resource
            var bookResource = new BookResource
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Author = newBook.Author
            };


            return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, bookResource);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookResource>> PutBooks(int id, [FromBody] BookModel bookModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // You can make it like Yazan said from his document.
            var bookToUpdate = await _bookRepository?.Get(id);

            bookToUpdate.Title = bookModel.Title;
            bookToUpdate.Author = bookModel.Author;
            bookToUpdate.Description = bookModel.Description;


            if (bookToUpdate == null)
                return NotFound();
            await _bookRepository.Update(bookToUpdate);
            var bookResource = new BookResource
            {
                Id = bookToUpdate.Id,
                Title = bookToUpdate.Title,
                Author = bookToUpdate.Author
            };
            JObject obj = (JObject)JToken.FromObject(bookResource);
            Console.WriteLine(bookResource);
            return Ok(bookResource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var bookToDelete = await _bookRepository.Get(id);
            if (bookToDelete == null)
                return NotFound();

            await _bookRepository.Delete(bookToDelete.Id);
            return NoContent();
        }
    }
}
