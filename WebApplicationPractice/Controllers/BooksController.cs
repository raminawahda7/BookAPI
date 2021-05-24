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
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Book, int> _bookRepository;
        private readonly IRepository<Author, int> _authorRepository;
        private readonly IRepository<Publisher, int> _publisherRepository;
        public BooksController(IRepository<Book, int> bookRepository,
            IRepository<Author, int> authorRepository
            , IRepository<Publisher, int> publisherRepository)

        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<BookResource>> GetBooks(string author)
        {
            // To-Do: implement author parameter
            var entities = await _bookRepository.Get();
            //if (author != null)
            //    entities = entities.Where(e => e.Authors.Contains(author));
            var listOfBookResource = new List<BookResource>();

            foreach (var item in entities)
            {
                var bookResource = new BookResource {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    IsAvailable = item.IsAvailable,
                    Publisher = item.Publisher.Name,
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
                Description = bookFromRepo.Description,
                IsAvailable = bookFromRepo.IsAvailable,
                Publisher = bookFromRepo.Publisher.Name,
                //AuthorNames = bookFromRepo.Authors.Select(e => e.FullName).ToList()
            };

            return Ok(bookResource);
        }


        [HttpPost]
        public async Task<ActionResult<BookResource>> PostBook([FromBody] BookModel bookModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Here map (model) -> entity
            var bookEntity = new Book
            {
                Title = bookModel.Title,
                Description = bookModel.Description,
                IsAvailable = bookModel.IsAvailable,
                PublisherId=bookModel.PublisherId,

                //Authors = bookModel.AuthorIds.Select(id=> new Author { Id = id }).ToList()
            };

            // insert this record to database by repo
            var newBook = await _bookRepository.Create(bookEntity);

            // Here map (newBook which is Entity) -> Resource
            var bookResource = new BookResource
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Description = newBook.Description,
                IsAvailable = newBook.IsAvailable,
                Publisher = newBook.Publisher.Name,
                //AuthorNames = newBook.Authors.Select(e=>  e.FullName ).ToList()
            };


            return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, bookResource);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookResource>> PutBook(int id, [FromBody] BookModel bookModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // You can make it like Yazan said from his document.
            var bookToUpdate = await _bookRepository?.Get(id);

            bookToUpdate.Title = bookModel.Title;
            bookToUpdate.Description = bookModel.Description;
            bookToUpdate.IsAvailable = bookModel.IsAvailable;
            bookToUpdate.PublisherId = bookModel.PublisherId;
            //bookToUpdate.Authors = bookModel.AuthorIds.Select(id => new Author { Id = id }).ToList();

            if (bookToUpdate == null)
                return NotFound();
            await _bookRepository.Update(bookToUpdate);
            var bookResource = new BookResource
            {
                Id = bookToUpdate.Id,
                Title = bookToUpdate.Title,
                Description = bookToUpdate.Description,
                IsAvailable = bookToUpdate.IsAvailable,
                Publisher=bookToUpdate.Publisher.Name,
                //AuthorNames = bookToUpdate.Authors.Select(e=>  e.FullName ).ToList()
            };
            //JObject obj = (JObject)JToken.FromObject(bookResource);
            //Console.WriteLine(bookResource);
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
