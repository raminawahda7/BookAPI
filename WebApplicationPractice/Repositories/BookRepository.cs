using BookAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        // ??????????
        private readonly BookContext _context;
        public BookRepository(BookContext context)
        {
            _context = context;
        }

        public async Task<Book> Create(Book book)
        {
            // we are using Add method to add a new instance of the book class.
            _context.BookStore.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task Delete(int id)
        {
            var bookToDelete =await _context.BookStore.FindAsync(id);
            _context.BookStore.Remove(bookToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> Get()
        {
            return await _context.BookStore.ToListAsync();
        }

        public async Task<Book> Get(int id)
        {
            return await _context.BookStore.FindAsync(id);
        }

        public async Task<Book> Update(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
