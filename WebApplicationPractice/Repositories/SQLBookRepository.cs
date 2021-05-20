using BookAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly BookContext _bookContext;

        public SQLBookRepository(BookContext bookContext)
        {
            _bookContext = bookContext;
        }
        public async Task<Book> Create(Book book)
        {
            _bookContext.BookStore.Add(book);
            await _bookContext.SaveChangesAsync();
            return book;
        }

        public async Task Delete(int id)
        {
            // check if there is book for this id : then store it in a variable using await keywprd ((( that is mean wait till find the book THEN put it in a variable)));
            var bookToDelete = await _bookContext.BookStore.FindAsync(id);
            _bookContext.BookStore.Remove(bookToDelete);
            await _bookContext.SaveChangesAsync();

        }

        public async Task<IEnumerable<Book>> Get()
        {
            return await _bookContext.BookStore.ToListAsync();
        }

        public async Task<Book> Get(int id)
        {
            return await _bookContext.BookStore.FindAsync(id);
        }

        public async Task<Book> Update(Book book)
        {
            _bookContext.Entry(book).State = EntityState.Modified;
            await _bookContext.SaveChangesAsync();
            return book;
        }
    }
}
