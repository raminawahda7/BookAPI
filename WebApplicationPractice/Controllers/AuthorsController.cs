using BookAPI.Data;
using BookAPI.Helper;
using BookAPI.Repositories.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        public readonly IManager _authorManager;
        public AuthorsController(IManager authorManager)

        {
            _authorManager = authorManager;

        }


        // TO-Do: add a new author_resource contains List of Book-Titles field.
        // Then change the resource for GetAuthors to return authors with what they wrote :)
        [HttpGet]
        public async Task<IActionResult> GetAuthors(string author)
        {
            var authors = await _authorManager.GetAuthors(author);
         
            return Ok(authors);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await _authorManager.GetAuthor(id);

            return Ok(author);
        }


        [HttpPost]
        public async Task<IActionResult> PostAuthor([FromBody] AuthorModel authorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var author = await _authorManager.PostAuthor(authorModel);

            return Ok(author);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, [FromBody] AuthorModel authorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = await _authorManager.PutAuthor(id,authorModel);

            return Ok(author);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var authorToDelete = await _authorManager.DeleteAuthor(id);
            if (authorToDelete == null)
                return NoContent();
            else
                return BadRequest();
        }


    }
}
