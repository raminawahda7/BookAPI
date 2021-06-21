using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Services
{
    public interface IAuthorPublisherServices
    {
        Task CreateAuthor(int Id);
        Task UpdateAuthor(int Id);
        Task DeleteAuthor(int Id);
        Task CreatePublisher(int Id);
        Task UpdatePublisher(int Id);
        Task DeletePublisher(int Id);
        Task PublisherHarvest();
        Task AuthorHarvest();

    }
}
