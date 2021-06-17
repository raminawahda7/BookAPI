using BookAPI.Data;
using BookAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    public class SQLPublisherRepository : IRepository<Publisher, int>
    {
        private readonly AppDbContext _Context;

        public SQLPublisherRepository(AppDbContext Context)
        {
            _Context = Context;
        }
        public async Task<Publisher> Create(Publisher publisher)
        {
            if (publisher == null)
            {
                throw new ArgumentNullException($"{nameof(Create)} entity must not be null");
            }
            try
            {
                await _Context.Publisher.AddAsync(publisher);
                await _Context.SaveChangesAsync();

                return await _Context.Publisher.Include(e => e.Books).FirstOrDefaultAsync(e => e.Id == publisher.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't create publisher :{ex.Message}");
            }
        }

        public async Task<Exception> Delete(int id)
        {
            try
            {
                // check if there is book for this id : then store it in a variable using await keywprd ((( that is mean wait till find the book THEN put it in a variable)));
                var publisherToDelete = await _Context.Publisher.FindAsync(id);
                _Context.Publisher.Remove(publisherToDelete);
                await _Context.SaveChangesAsync();
                return null;

            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete publisher :{ex.Message}");
            }

        }

        public async Task<IEnumerable<Publisher>> Get()
        {
            try
            {
                return await _Context.Publisher.Include(p => p.Books).ThenInclude(b => b.Authors).ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve publisher :{ex.Message}");
            }
            //return await _Context.Publishers.Include(b => b.Books).ToListAsync();
        }

        public async Task<Publisher> Get(int id)
        {
            try
            {
                return await _Context.Publisher.Include(p => p.Books).ThenInclude(b => b.Authors).FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve publisher :{ex.Message}");
            }
        }

        public async Task<Publisher> Update(Publisher publisher)
        {
            if (publisher == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} entity must not be null");
            }
            try
            {

            _Context.Entry(publisher).State = EntityState.Modified;
            await _Context.SaveChangesAsync();
            return publisher;
            }
            catch(Exception ex)
            {
               throw new Exception($"Couldn't update publisher :{ex.Message}");
            }
        }
    }
}
