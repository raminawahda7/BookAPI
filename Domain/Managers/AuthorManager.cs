using BookAPI.Data;
using BookAPI.Helper;
using BookAPI.Repositories;
using Domain.Managers.Sender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Managers
{
    public interface IAuthorManager
    {
        public Task<IEnumerable<AuthorResource>> GetAuthors(string author);
        public Task<AuthorResource> GetAuthor(int id);
        public Task<AuthorCreateResource> PostAuthor(AuthorModel authorModel);
        public Task<AuthorCreateResource> PutAuthor(int id, AuthorModel authorModel);
        public Task<Exception> DeleteAuthor(int id);

    }
    public class AuthorManager : IAuthorManager
    {
        private readonly IRepository<Author, int> _authorRepository;
        private readonly ISender _sender;

        public AuthorManager(IRepository<Author, int> authorRepository, ISender Sender)
        {
            _authorRepository = authorRepository;
            _sender = Sender;
        }
        public async Task<Exception> DeleteAuthor(int id)
        {
            var authorToDelete = await _authorRepository.Get(id);
            if (authorToDelete == null)
                throw new Exception("Id is not found");


            //var authorObj = new toSend()
            //{
            //    Id = authorToDelete.Id,
            //    Type = "delete"
            //};
            _sender.SendAuthor(new toSend()
            {
                Id = authorToDelete.Id,
                Type = "delete"
            });

            return await _authorRepository.Delete(authorToDelete.Id);
        }


        public async Task<AuthorResource> GetAuthor(int id)
        {
            var authorFromRepo = await _authorRepository.Get(id);
            if (authorFromRepo == null)
                throw new Exception("Id is not found");
            //var books = _bookRepository.Get().Result.Where(e => bookModel.AuthorIds.Contains(e.Id)).ToList();

            var authorResource = new AuthorResource
            {
                Id = authorFromRepo.Id,
                FullName = authorFromRepo.FullName,
                Age = authorFromRepo.Age,
                Email = authorFromRepo.Email,
                Books = new List<BookAuthorResource>()
            };
            authorResource.Books = authorFromRepo.Books.Select(book => new BookAuthorResource
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                IsAvailable = book.IsAvailable,
            }).ToList();

            return authorResource;
        }

        public async Task<IEnumerable<AuthorResource>> GetAuthors(string author)
        {
            var entities = await _authorRepository.Get();
            if (author != null)
            {
                entities = entities.Where(e => e.FullName.Contains(author));
            }
            return entities.AuthorBookResource();
        }


        public async Task<AuthorCreateResource> PostAuthor(AuthorModel authorModel)
        {

            var authorEntity = new Author
            {
                FirstName = authorModel.FirstName,
                LastName = authorModel.LastName,

            };
            if (authorModel.Email == null && authorModel.Age == null)
                throw new("At least some name must be added");
            authorEntity.Email = authorModel.Email;
            authorEntity.Age = authorModel.Age;
            // Entity from Book
            var newAuthor = await _authorRepository.Create(authorEntity);
            _sender.SendAuthor(new toSend()
            {
                Id = newAuthor.Id,
                Type = "create"
            });
            // Here map (newBook which is Entity) -> Resource
            var authorResource = new AuthorCreateResource
            {
                Id = newAuthor.Id,
                FullName = newAuthor.FullName,
                //Age = newAuthor.Age,
                //Email = newAuthor.Email
            };

            return authorResource;
        }


        public async Task<AuthorCreateResource> PutAuthor(int id, AuthorModel authorModel)
        {
            // You can make it like Yazan said from his document.
            var authorToUpdate = await _authorRepository.Get(id);

            if (authorToUpdate == null)
                throw new Exception("Author is not found");

            authorToUpdate.FirstName = authorModel.FirstName;
            authorToUpdate.LastName = authorModel.LastName;
            authorToUpdate.Email = authorModel.Email;
            authorToUpdate.Age = authorModel.Age;

            await _authorRepository.Update(authorToUpdate);


            _sender.SendAuthor(new toSend()
            {
                Id = authorToUpdate.Id,
                Type = "update"
            });

            var authorResource = new AuthorCreateResource
            {
                Id = authorToUpdate.Id,
                FullName = authorToUpdate.FullName,
                //Age = authorToUpdate.Age,
                //Email = authorToUpdate.Email
            };
            return authorResource;
        }


    }
}
