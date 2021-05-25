using BookAPI.Data;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class SQLAuthorRepository : IRepository<Author,int>
    {
        private readonly AppDbContext _Context;

        public SQLAuthorRepository(AppDbContext Context)
        {
            _Context = Context;
        }
        public async Task<Author> Create(Author author)
        {
             _Context.Authors.Add(author);
            await _Context.SaveChangesAsync();

            return author;
        }

        public async Task Delete(int id)
        {
            // check if there is book for this id : then store it in a variable using await keywprd ((( that is mean wait till find the book THEN put it in a variable)));
            var bookToDelete = await _Context.Authors.FindAsync(id);
            _Context.Authors.Remove(bookToDelete);
            await _Context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Author>> Get()
        {
            return await _Context.Authors.Include(b => b.Books).ToListAsync();
        }

        public async Task<Author> Get(int id)
        {
            return await _Context.Authors.FindAsync(id);
        }

        public async Task<Author> Update(Author book)
        {
            _Context.Entry(book).State = EntityState.Modified;
            await _Context.SaveChangesAsync();
            return book;
        }
    }
}
