using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Repositories
{
    interface IBookRepository
    {
        Task<IEnumerable <Book>>get(); // retrieve all books
        // get with id parameter will retrieve a single book then create, update and delete
        Task<Book> Get(int id);
        Task<Book> Create(Book book);
        Task<Book> Update(Book book);
        Task Delete(int id); 

    }
}
