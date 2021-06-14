using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IAuthorManager
    {
        public Task<IEnumerable<AuthorResource>> GetAuthors(string author);
        public Task<AuthorResource> GetAuthor(int id);
        public Task<AuthorCreateResource> PostAuthor(AuthorModel authorModel);
        public Task<AuthorCreateResource> PutAuthor(int id, AuthorModel authorModel);
        public Task<Exception> DeleteAuthor(int id);

    }
}
