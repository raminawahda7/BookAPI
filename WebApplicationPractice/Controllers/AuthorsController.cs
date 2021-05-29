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
        private readonly IRepository<Author, int> _authorRepository;
        private readonly IRepository<Book, int> _bookRepository;

        public AuthorsController(IRepository<Author, int> authorRepository, IRepository<Book, int> bookRepository)

        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;

        }


        // TO-Do: add a new author_resource contains List of Book-Titles field.
        // Then change the resource for GetAuthors to return authors with what they wrote :)
        [HttpGet]
        public async Task<IEnumerable<AuthorResource>> GetAuthors(string author)
        {
            var entities = await _authorRepository.Get();
            var listOfAuthorResource = new List<AuthorResource>();

            //if (author != null)
            //    entities = entities.Where(e => e.Author.Contains(author));
            foreach (var entity in entities)
            {
                var resource = new AuthorResource();
                resource.Id = entity.Id;
                resource.FullName = entity.FullName;
                resource.Age = entity.Age;
                resource.Email = entity.Email;
                resource.Books = new List<BookAuthorResource>();
                foreach (var book in entity.Books)
                {
                    resource.Books.Add(new BookAuthorResource
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = book.Description,
                        IsAvailable = book.IsAvailable,
                    });
                }
                listOfAuthorResource.Add(resource);
            }
            return listOfAuthorResource;

        }



        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResource>> GetAuthors(int id)
        {
            var authorFromRepo = await _authorRepository.Get(id);
            if (authorFromRepo == null)
            {
                return NotFound();
            }
            //var books = _bookRepository.Get().Result.Where(e => bookModel.AuthorIds.Contains(e.Id)).ToList();

            var authorResource = new AuthorResource
            {
                Id = authorFromRepo.Id,
                FullName = authorFromRepo.FullName,
                Age=authorFromRepo.Age,
                Email = authorFromRepo.Email
                //BookTitles = authorFromRepo.Books.Select(e => e.Title).ToList()
            };

            return Ok(authorResource);
        }


        [HttpPost]
        public async Task<ActionResult<AuthorCreateResource>> PostAuthor([FromBody] AuthorModel authorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Here map (model) -> entity
            var authorEntity = new Author
            {
                FirstName = authorModel.FirstName,
                LastName = authorModel.LastName,
                Email = authorModel.Email,
                Age = authorModel.Age
            };
            // Entity from Book
            var newAuthor = await _authorRepository.Create(authorEntity);

            // Here map (newBook which is Entity) -> Resource
            var authorResource = new AuthorCreateResource
            {
                Id = newAuthor.Id,
                FullName = newAuthor.FullName,
                //Age = newAuthor.Age,
                //Email = newAuthor.Email

            };


            return CreatedAtAction(nameof(GetAuthors), new { id = newAuthor.Id }, authorResource);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorCreateResource>> PutAuthor(int id, [FromBody] AuthorModel authorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // You can make it like Yazan said from his document.
            var authorToUpdate = await _authorRepository.Get(id);

            if (authorToUpdate == null)
                return NotFound();

            authorToUpdate.FirstName = authorModel.FirstName;
            authorToUpdate.LastName = authorModel.LastName;
            authorToUpdate.Email = authorModel.Email;
            authorToUpdate.Age = authorModel.Age;


            await _authorRepository.Update(authorToUpdate);
            var authorResource = new AuthorCreateResource
            {
                Id = authorToUpdate.Id,
                FullName = authorToUpdate.FullName,
                //Age = authorToUpdate.Age,
                //Email = authorToUpdate.Email
            };
            //JObject obj = (JObject)JToken.FromObject(bookResource);
            //Console.WriteLine(bookResource);
            return Ok(authorResource);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var authorToDelete = await _authorRepository.Get(id);
            if (authorToDelete == null)
                return NotFound();

            await _authorRepository.Delete(authorToDelete.Id);
            return NoContent();
        }


    }
}
