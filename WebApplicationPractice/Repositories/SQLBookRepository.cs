using BookAPI.Data;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class SQLBookRepository : IRepository<Book,int>
    {
        private readonly AppDbContext _Context;

        public SQLBookRepository(AppDbContext Context)
        {
            _Context = Context;
        }
        public async Task<Book> Create(Book book)
        {
             _Context.Book.Add(book);
            await _Context.SaveChangesAsync();

            return book;
        }

        public async Task Delete(int id)
        {
            // check if there is book for this id : then store it in a variable using await keywprd ((( that is mean wait till find the book THEN put it in a variable)));
            var bookToDelete = await _Context.Book.FindAsync(id);
            _Context.Book.Remove(bookToDelete);
            await _Context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Book>> Get()
        {
            return await _Context.Book.Include(b=>b.Publisher).ToListAsync();
        }

        public async Task<Book> Get(int id)
        {
            return await _Context.Book.Include(b=>b.Publisher).FirstOrDefaultAsync(e=>e.Id==id);
        }

        public async Task<Book> Update(Book book)
        {
            _Context.Entry(book).State = EntityState.Modified;
            await _Context.SaveChangesAsync();
            return book;
        }
    }
}
