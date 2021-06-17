using BookAPI.Data;
using BookAPI.Helper;
using BookAPI.Repositories;
using Domain.Managers.Sender;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Managers
{
    public interface IPublisherManager
    {
        public Task<IEnumerable<PublisherResource>> GetPublishers();
        public Task<PublisherResource> GetPublisher(int id);
        public Task<PublisherCreateResource> PostPubliser(PublisherModel publisherModel);
        public Task<PublisherCreateResource> PutPublihser(int id, PublisherModel publisherModel);
        public Task<Exception> DeletePublisher(int id);
    }

    public  class PublisherManager : IPublisherManager
    {
        private readonly IRepository<Publisher,int> _publisherRepository;
        private readonly ISender _sender;
        public PublisherManager(IRepository<Publisher, int> publisherRepository, ISender Sender)
        {
            _publisherRepository = publisherRepository;
            _sender = Sender;
        }
        

        #region DeletePublisher
        public async Task<Exception> DeletePublisher(int id)
        {
            var publisherToDelete = await _publisherRepository.Get(id);

            if (publisherToDelete == null)
                throw new Exception("Id is not found");
            if (publisherToDelete.Books.Count == 0)
            {
                _sender.SendAuthorPublisher(new toSend()
                {
                    Id = publisherToDelete.Id,
                    entityType = "publisher",
                    ProcessType = "delete"
                });
                return await _publisherRepository.Delete(publisherToDelete.Id);
            }
            else
                throw new("Can't delete publisher has book");
        }
        #endregion

        #region GetPublisherId
        public async Task<PublisherResource> GetPublisher(int id)
        {
            var publisherFromRepo = await _publisherRepository.Get(id);
            if (publisherFromRepo == null)
                throw new Exception("Id is not found");
          
            var resource = new List<PublisherBookResource>();
            foreach (var book in publisherFromRepo.Books)
            {
                resource.Add(new PublisherBookResource
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsAvailable = book.IsAvailable,
                    //AuthorNames = book.Authors.Select(e => e.FullName).ToList()
                });
            }
            var publisherResource = new PublisherResource
            {
                Id = publisherFromRepo.Id,
                Name = publisherFromRepo.Name,
                Books = resource
            };

            return publisherResource;
        }
        #endregion

        #region GetPublishers
        public async Task<IEnumerable<PublisherResource>> GetPublishers()
        {
            var entities = await _publisherRepository.Get();

            return entities.PublisherBookResource();
        }
        #endregion

        #region PostPublisher
        public async Task<PublisherCreateResource> PostPubliser(PublisherModel publisherModel)
        {
            var publisherEntity = new Publisher
            {
                Name = publisherModel.Name,
            };
            // Entity from Book
            var newPublisher = await _publisherRepository.Create(publisherEntity);

            // Here map (newBook which is Entity) -> Resource
            var publisherResource = new PublisherCreateResource
            {
                Id = newPublisher.Id,
                Name = newPublisher.Name,
            };
     
            _sender.SendAuthorPublisher(new toSend()
            {
                Id = newPublisher.Id,
                entityType = "publisher",
                ProcessType = "create"
            });

            return publisherResource;
        }
        #endregion

        public async Task<PublisherCreateResource> PutPublihser(int id, PublisherModel publisherModel)
        {
            // You can make it like Yazan said from his document.
            var publisherToUpdate = await _publisherRepository.Get(id);

            if (publisherToUpdate == null)
                throw new Exception("Publisher is not found");

            publisherToUpdate.Name = publisherModel.Name;

            await _publisherRepository.Update(publisherToUpdate);
            var publisherResource = new PublisherCreateResource
            {
                Id = publisherToUpdate.Id,
                Name = publisherToUpdate.Name,
            };
            _sender.SendAuthorPublisher(new toSend()
            {
                Id = publisherToUpdate.Id,
                entityType = "publisher",
                ProcessType = "update"
            });
            return publisherResource;
        }
    }
}
