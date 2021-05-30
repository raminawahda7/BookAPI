using BookAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookAPI.Helper
{
    public static class Extensions
    {
        public static List<AuthorResource> AuthorBookResource(this IEnumerable<Author> entity)
        {

            var listOfAuthorResource = new List<AuthorResource>();

            //if (author != null)
            //    entities = entities.Where(e => e.Author.Contains(author));
            foreach (var author in entity)
            {
                var resource = new AuthorResource();
                resource.Id = author.Id;
                resource.FullName = author.FullName;
                resource.Age = author.Age;
                resource.Email = author.Email;
                resource.Books = new List<BookAuthorResource>();
                foreach (var book in author.Books)
                {
                    resource.Books.Add(new BookAuthorResource
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = book.Description,
                        IsAvailable = book.IsAvailable,
                    });
                }
                listOfAuthorResource.Add(resource);
            }

            return listOfAuthorResource;
        }
        public static List<BookResource> BookAuthorResource (this IEnumerable<Book> entity)
        {
            var lsitOfBookResource = new List<BookResource>();
          

            foreach (var book in entity)
            {
                var bookResource = new BookResource
                {
                   Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsAvailable = book.IsAvailable,
                    Publisher = book.Publisher.Name,
                    PublishedDate = book.PublishedDate,
                    AuthorNames = new List<AuthorCreateResource>(),
                };

                foreach (var author in book.Authors)
                {
                    bookResource.AuthorNames.Add(new AuthorCreateResource
                    {
                        Id= author.Id,
                        FullName = author.FullName
                    });
                }
                lsitOfBookResource.Add(bookResource);
            }
            return lsitOfBookResource;
        }

        public static List<PublisherResource> PublisherBookResource(this IEnumerable<Publisher> entity)
        {
            var listOfPublisherResource = new List<PublisherResource>();
            foreach (var publisher in entity)
            {
                var publisherResource = new PublisherResource
                {
                    Id = publisher.Id,
                    Name = publisher.Name,
                    Books = new List<PublisherBookResource>(),
                };

                foreach (var book in publisher.Books)
                {
                    var bookPublisher = new PublisherBookResource
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = book.Description,
                        IsAvailable = book.IsAvailable,
                        AuthorNames = new List<AuthorCreateResource>(),
                    };

                    foreach (var author in book.Authors)
                    {
                        bookPublisher.AuthorNames.Add(new AuthorCreateResource
                        {
                            Id = author.Id,
                            FullName = author.FullName
                        });
                    }
                    publisherResource.Books.Add(bookPublisher);
                }
                listOfPublisherResource.Add(publisherResource);
            }
            return listOfPublisherResource;
        }
    }
}
