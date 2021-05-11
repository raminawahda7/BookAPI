﻿using BookAPI.Models;
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
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> get()
        {
            throw new NotImplementedException();
        }

        public Task<Book> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book> Update(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
