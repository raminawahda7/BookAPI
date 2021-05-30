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
        public async Task<IEnumerable<BookResource>> GetBooks(string book)
        {
            // To-Do: implement author parameter
            var entities = await _bookRepository.Get();
            if (book != null)
            {
                //entities = entities.Where(e => e.Title.Contains(book));
                entities = entities.Where(e => e.Authors.Where(a=>a.FullName.Contains(book)));
            }
            //if (author != null)
            //    entities = entities.Where(e => e.Authors.Where(a=>a.FullName==author));

            return entities.BookAuthorResource();

        }



        [HttpGet("{id}")]
        public async Task<ActionResult<BookResource>> GetBooks(int id)
        {
            var bookFromRepo = await _bookRepository.Get(id);
            if (bookFromRepo == null)
            {
                return NotFound();
            }
            var listOfAuthorResource = new List<AuthorCreateResource>();

            foreach (var author in bookFromRepo.Authors)
            {
                var authorResource = new AuthorCreateResource
                {
                    Id = author.Id,
                    FullName = author.FullName
                };
                listOfAuthorResource.Add(authorResource);
            }


            var bookResource = new BookResource
            {
                Id = bookFromRepo.Id,
                Title = bookFromRepo.Title,
                Description = bookFromRepo.Description,
                IsAvailable = bookFromRepo.IsAvailable,
                Publisher = bookFromRepo.Publisher.Name,
                PublishedDate = bookFromRepo.PublishedDate,
                AuthorNames = listOfAuthorResource
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
            var authors = _authorRepository.Get().Result.ToList().Where(e => bookModel.AuthorIds.Contains(e.Id)).ToList();
            if (authors.Count < bookModel.AuthorIds.Count) throw new Exception("Id is not correct");
            
            var bookEntity = new Book
            {
                Title = bookModel.Title,
                Description = bookModel.Description,
                IsAvailable = bookModel.IsAvailable,
                PublisherId = bookModel.PublisherId,
                PublishedDate = bookModel.PublishedDate,
                Authors = authors
            };

            // insert this record to database by repo
            var newBook = await _bookRepository.Create(bookEntity);
            // map author list into book resource
            var listOfAuthorResource = new List<AuthorCreateResource>();

            foreach (var author in authors)
            {
                var authorResource = new AuthorCreateResource
                {
                    Id = author.Id,
                    FullName = author.FullName
                };
                listOfAuthorResource.Add(authorResource);
            }

            // Here map (newBook which is Entity) -> Resource
            var bookResource = new BookResource
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Description = newBook.Description,
                IsAvailable = newBook.IsAvailable,
                Publisher = newBook.Publisher.Name,
                PublishedDate = newBook.PublishedDate,
                AuthorNames = listOfAuthorResource
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
            var bookToUpdate = await _bookRepository.Get(id);

            if (bookToUpdate == null)
                return NotFound();

            var authors = _authorRepository.Get().Result.ToList().Where(e => bookModel.AuthorIds.Contains(e.Id)).ToList();

            if (authors.Count < bookModel.AuthorIds.Count) throw new Exception("Id is not correct");

            bookToUpdate.Title = bookModel.Title;
            bookToUpdate.Description = bookModel.Description;
            bookToUpdate.IsAvailable = bookModel.IsAvailable;
            bookToUpdate.PublisherId = bookModel.PublisherId;
            bookToUpdate.PublishedDate = bookModel.PublishedDate;
            bookToUpdate.Authors = authors;

            if (bookToUpdate == null)
                return NotFound();
            await _bookRepository.Update(bookToUpdate);
            // map authors list into book resource
            var listOfAuthorResource = new List<AuthorCreateResource>();

            foreach (var author in authors)
            {
                var authorResource = new AuthorCreateResource
                {
                    Id = author.Id,
                    FullName = author.FullName
                };
                listOfAuthorResource.Add(authorResource);
            }

            var bookResource = new BookResource
            {
                Id = bookToUpdate.Id,
                Title = bookToUpdate.Title,
                Description = bookToUpdate.Description,
                IsAvailable = bookToUpdate.IsAvailable,
                Publisher = bookToUpdate.Publisher.Name,
                PublishedDate = bookToUpdate.PublishedDate,
                AuthorNames = listOfAuthorResource
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
