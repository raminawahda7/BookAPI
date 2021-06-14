using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IPublisherManager
    {
        public Task<IEnumerable<PublisherResource>> GetPublishers();
        public Task<PublisherResource> GetPublisher(int id);
        public Task<PublisherCreateResource> PostPubliser(PublisherModel publisherModel);
        public Task<PublisherCreateResource> PutPublihser(int id, PublisherModel publisherModel);
        public Task<Exception> DeletePublisher(int id);
    }
}
