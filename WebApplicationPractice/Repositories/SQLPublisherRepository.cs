using BookAPI.Data;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class SQLPublisherRepository : IRepository<Publisher,int>
    {
        private readonly AppDbContext __Context;

        public SQLPublisherRepository(AppDbContext Context)
        {
            __Context = Context;
        }
        public async Task<Publisher> Create(Publisher publisher)
        {
            await __Context.Publishers.AddAsync(publisher);
            //__Context.SaveChangesAsync();

            return publisher;
        }

        public async Task Delete(int id)
        {
            // check if there is book for this id : then store it in a variable using await keywprd ((( that is mean wait till find the book THEN put it in a variable)));
            var bookToDelete = await __Context.Publishers.FindAsync(id);
            __Context.Publishers.Remove(bookToDelete);
            await __Context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Publisher>> Get()
        {
            return await __Context.Publishers.ToListAsync();
        }

        public async Task<Publisher> Get(int id)
        {
            return await __Context.Publishers.FindAsync(id);
        }

        public async Task<Publisher> Update(Publisher book)
        {
            __Context.Entry(book).State = EntityState.Modified;
            await __Context.SaveChangesAsync();
            return book;
        }
    }
}
