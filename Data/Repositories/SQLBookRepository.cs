using BookAPI.Data;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class SQLBookRepository : IRepository<Book, int>
    {
        private readonly AppDbContext _Context;

        public SQLBookRepository(AppDbContext Context)
        {
            _Context = Context;
        }
        public async Task<Book> Create(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException($"{nameof(Create)} entity must not be null");
            }

            _Context.Book.Add(book);

            //_Context.Authors.AttachRange(book.Authors);
            await _Context.SaveChangesAsync();
            return await _Context.Book.Include(b => b.Publisher).Include(b => b.Authors).FirstOrDefaultAsync(e => e.Id == book.Id);
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
            try
            {
                return await _Context.Book.Include(b => b.Publisher).Include(b => b.Authors).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($" Couldn't retrieve book {ex.Message}");
            }
        }

        public async Task<Book> Get(int id)
        {
            try
            {
                return await _Context.Book.Include(b => b.Publisher).Include(b => b.Authors).FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($" Couldn't retrieve book {ex.Message}");
            }
        }

        public async Task<Book> Update(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} entity must not be null");
            }
            try
            {
                _Context.Entry(book).State = EntityState.Modified;
                await _Context.SaveChangesAsync();
                return book;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't update book :{ex.Message}");
            }
        }
    }
}
