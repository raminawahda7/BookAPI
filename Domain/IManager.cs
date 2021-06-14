using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IManager
    {
        #region Authors
        public Task<IEnumerable<AuthorResource>> GetAuthors(string author);
        public Task<AuthorResource> GetAuthor(int id);
        public Task<AuthorCreateResource> PostAuthor(AuthorModel authorModel);
        public Task<AuthorCreateResource> PutAuthor(int id, AuthorModel authorModel);
        public Task<Exception> DeleteAuthor(int id);
        #endregion

        #region Publishers
        public Task<IEnumerable<PublisherResource>> GetPublishers();
        public Task<PublisherResource> GetPublisher(int id);
        public Task<PublisherCreateResource> PostPubliser(PublisherModel publisherModel);
        public Task<PublisherCreateResource> PutPublihser(int id, PublisherModel publisherModel);
        public Task<Exception> DeletePublisher(int id);



        #endregion

    }
}
