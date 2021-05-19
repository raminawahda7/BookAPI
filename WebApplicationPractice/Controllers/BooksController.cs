using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _bookRepository.Get();

        }

        //// ------- GetBook with special Dto:


        //[HttpGet("{id}")]
        //public async Task<ActionResult<Book>> GetBooks(int id)
        //{
        //    return await _bookRepository.Get(id);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBooks(int id)
        {
            var bookFromRepo = await _bookRepository.Get(id);
            if (bookFromRepo == null)
            {
                return NotFound();
            }
            var BookDto = new BookDto
            {
                Id = bookFromRepo.Id,
                Title = bookFromRepo.Title,
                Author = bookFromRepo.Author
            };

            return Ok(BookDto);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBooks([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newBook = await _bookRepository.Create(book);
            var BookDto = new BookDto
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Author = newBook.Author
            };
            return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, BookDto);

        }

        [HttpPut]
        public async Task<ActionResult> PutBooks(int id, [FromBody] Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            await _bookRepository.Update(book);

            return ValidationProblem();
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
