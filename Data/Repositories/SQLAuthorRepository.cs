using BookAPI.Data;
using BookAPI.Repositories;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class SQLAuthorRepository : IRepository<Author, int>
    {
        private readonly AppDbContext _Context;

        public SQLAuthorRepository(AppDbContext Context)
        {
            _Context = Context;
        }
        public async Task<Author> Create(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException($"{nameof(Create)} entity must not be null");
            }
            try
            {
                //------------------------------------
                _Context.Author.Add(author);
                await _Context.SaveChangesAsync();

                return author;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't create author :{ex.Message}");
            }
        }

        public async Task<Exception> Delete(int id)
        {

            // check if there is book for this id : then store it in a variable using await keywprd ((( that is mean wait till find the book THEN put it in a variable)));
            try
            {
                var bookToDelete = await _Context.Author.FindAsync(id);
                _Context.Author.Remove(bookToDelete);
                await _Context.SaveChangesAsync();
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete author :{ex.Message}");
            }
        }

        public async Task<IEnumerable<Author>> Get()
        {
            try
            {
                return await _Context.Author.Include(b => b.Books).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve authors :{ex.Message}");
            }
        }

        public async Task<Author> Get(int id)
        {
            try
            {
                return await _Context.Author.Include(b => b.Books).FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve author  :{ex.Message}");
            }
        }

        public async Task<Author> Update(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} entity must not be null");
            }
            try
            {
                _Context.Entry(author).State = EntityState.Modified;

                await _Context.SaveChangesAsync();
                return author;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update author :{ex.Message}");
            }
        }
    }
}
