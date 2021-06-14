using BookAPI.Data;
using BookAPI.Helper;
using BookAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Managers
{
    public partial class AuthorManager : IAuthorManager
    {
        private readonly IRepository<Author, int> _authorRepository;
        private readonly IRepository<Publisher, int> _publisherRepository;

        public AuthorManager(IRepository<Author, int> authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<Exception> DeleteAuthor(int id)
        {
            var authorToDelete = await _authorRepository.Get(id);
            if (authorToDelete == null)
                throw new Exception("Id is not found");


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
                Email = authorFromRepo.Email
                //BookTitles = authorFromRepo.Books.Select(e => e.Title).ToList()
            };

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
