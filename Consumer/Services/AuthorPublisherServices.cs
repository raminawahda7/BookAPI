using BookAPI.Data;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Consumer.Services
{
    public class AuthorPublisherServices : IAuthorPublisherServices
    {
        private readonly IRepository<Author, int> _authorRepository;
        private readonly IRepository<Publisher, int> _publisherRepository;
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _Context;

        public AuthorPublisherServices(HttpClient httpClient, IRepository<Author, int> authorRepository, IRepository<Publisher, int> publisherRepository, AppDbContext Context)
        {
            _httpClient = httpClient;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
            _Context = Context;
        }
        string baseUrl = "https://localhost:44359/api/";

        public async Task CreateAuthor(int Id)
        {
            #region long way to get data with some operations:
            /*
            using var httpResponse = await _httpClient.GetAsync(baseUrl + "authors/" + Id, HttpCompletionOption.ResponseHeadersRead);
            httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299
            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();
                //Console.WriteLine(contentStream);

                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();

                try
                {
                    var author =  serializer.Deserialize<Author>(jsonReader);
                    Console.WriteLine(author);
                }
                catch (JsonReaderException)
                {
                    Console.WriteLine("Invalid JSON.");
                }
            }
            else
            {
                Console.WriteLine("HTTP Response was invalid and cannot be deserialised.");
            }
            */
            #endregion

            Uri geturi = new Uri(baseUrl + "authors/" + Id); //replace your url  

            var responseGet = await _httpClient.GetAsync(geturi, HttpCompletionOption.ResponseHeadersRead);
            Console.WriteLine($" is it json :::::::::::::: {responseGet}");

            string response = await responseGet.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<AuthorResource>(response);
            var names = data.FullName.Split(' ');

            var author = new Author()
            {
                Id = Id,
                FirstName = names[0],
                LastName = names[1],
                Age = data.Age,
                Email = data.Email
            };
            await _authorRepository.Create(author);
        }

        public async Task CreatePublisher(int Id)
        {
            Uri geturi = new(baseUrl + "publishers/" + Id); //replace your url  

            var responseGet = await _httpClient.GetAsync(geturi, HttpCompletionOption.ResponseHeadersRead);
            Console.WriteLine($" is it json :::::::::::::: {responseGet}");
            //var stream = await responseGet.Content.ReadAsStreamAsync();
            //var data = await JsonSerializer.Deserialize<Author>(stream);
            //var datas =  JsonConvert.DeserializeObject<Author>(responseGet);
            //await _authorRepository.Create();
            string response = await responseGet.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<PublisherResource>(response);
            var publisher = new Publisher()
            {
                Id = Id,
                Name = data.Name
            };
            await _publisherRepository.Create(publisher);
        }

        public async Task DeleteAuthor(int Id)
        {
            Uri geturi = new Uri(baseUrl + "authors/" + Id); //replace your url  

            var responseGet = await _httpClient.GetAsync(geturi, HttpCompletionOption.ResponseHeadersRead);
            if (responseGet != null)
            {
                await _authorRepository.Delete(Id);
            }
            else
            {
                throw new Exception("Id is not found ");
            }

        }

        public async Task DeletePublisher(int Id)
        {
            Uri geturi = new Uri(baseUrl + "publishers/" + Id); //replace your url  

            var responseGet = await _httpClient.GetAsync(geturi, HttpCompletionOption.ResponseHeadersRead);
            if (responseGet != null)
            {
                await _publisherRepository.Delete(Id);
            }
            else
            {
                throw new Exception("Id is not found ");
            }
        }

        public async Task UpdateAuthor(int Id)
        {
            Uri geturi = new(baseUrl + "authors/" + Id); //replace your url  

            var responseGet = await _httpClient.GetAsync(geturi, HttpCompletionOption.ResponseHeadersRead);
            Console.WriteLine($" is it json :::::::::::::: {responseGet}");

            string response = await responseGet.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<AuthorResource>(response);
            var names = data.FullName.Split(' ');

            var author = new Author()
            {
                Id = Id,
                FirstName = names[0],
                LastName = names[1],
                Age = data.Age,
                Email = data.Email
            };
            await _authorRepository.Update(author);
        }

        public async Task UpdatePublisher(int Id)
        {
            Uri geturi = new Uri(baseUrl + "publishers/" + Id); //replace your url  

            var responseGet = await _httpClient.GetAsync(geturi, HttpCompletionOption.ResponseHeadersRead);
            Console.WriteLine($" is it json :::::::::::::: {responseGet}");
            //var stream = await responseGet.Content.ReadAsStreamAsync();
            //var data = await JsonSerializer.Deserialize<Author>(stream);
            //var datas =  JsonConvert.DeserializeObject<Author>(responseGet);
            //await _authorRepository.Create();
            string response = await responseGet.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<PublisherResource>(response);
            var publisher = new Publisher()
            {
                Id = Id,
                Name = data.Name
            };
            await _publisherRepository.Update(publisher);
        }
        public async Task PublisherHarvest()
        {
            Uri getPublishersUri = new Uri(baseUrl + "Harvest/Harvest/publishers");
            var responseGetPublishers = await _httpClient.GetAsync(getPublishersUri, HttpCompletionOption.ResponseHeadersRead);
            string publishersResponse = await responseGetPublishers.Content.ReadAsStringAsync();
            var originalPublishers = JsonConvert.DeserializeObject<List<Publisher>>(publishersResponse);
            //---------------------------------------------------------------------------------------
            List<Publisher> bookPublishers = new List<Publisher>();

            bookPublishers = await _Context.Publisher.ToListAsync();
            List<Publisher> listOfPublishersToUpdate = new List<Publisher>();
            List<Publisher> listOfPublishersToAdd = new List<Publisher>();


            foreach (var publisher in originalPublishers)
            {
                foreach (var bookPublisher in bookPublishers)
                {
                    if (publisher.Id == bookPublisher.Id && publisher.Name != bookPublisher.Name)
                    {
                        listOfPublishersToUpdate.Add(publisher);
                    }
                }
            }
            if (listOfPublishersToUpdate.Any())
            {
                Console.WriteLine("----------------================= listOfPublishersToUpdate ");

                await _Context.Publisher.BulkUpdateAsync(listOfPublishersToUpdate);
            }


            //originalPublishers.Where(c => !bookPublishers.Any(b => b.CusId == c.Id));
            var query = originalPublishers
                .Where(c => !bookPublishers
                    .Select(b => b.Id)
                    .Contains(c.Id)
                );
            listOfPublishersToAdd.AddRange(query);

            if (listOfPublishersToAdd.Any())
            {
                Console.WriteLine("----------------================= listOfPublishersToAdd ");

                await _Context.Publisher.BulkInsertAsync(listOfPublishersToAdd);
            }
            var listOfPublishersToDelete = bookPublishers.Where(bookP => !originalPublishers.Select(originalP => originalP.Id).Contains(bookP.Id));
            // delete query
            if (listOfPublishersToDelete.Any())
            {
                Console.WriteLine("----------------================= listOfPublishersToDelete ");

                await _Context.Publisher.BulkDeleteAsync(listOfPublishersToDelete);
            }
            // TODO: check if bookPulbisher is empty -> count =0 -> listToAdd = originalPublisher
            // TODO: update status
            // TODO: remove repo injection and replace it with dbContext.


        }
        public async Task AuthorHarvest()
        {
            Uri getAuthorsUri = new Uri(baseUrl + "Harvest/Harvest/authors");
            var responseGetAuthors = await _httpClient.GetAsync(getAuthorsUri, HttpCompletionOption.ResponseHeadersRead);
            string authorsResponse = await responseGetAuthors.Content.ReadAsStringAsync();
            var originalAuthors = JsonConvert.DeserializeObject<List<Author>>(authorsResponse);
            //---------------------------------------------------------------------------------------
            List<Author> bookAuthors = new List<Author>();

            bookAuthors = await _Context.Author.ToListAsync();
            List<Author> listOfAuthorsToUpdate = new List<Author>();
            List<Author> listOfAuthorsToAdd = new List<Author>();

            foreach (var author in originalAuthors)
            {
                foreach (var bookAuthor in bookAuthors)
                {
                    if (author.Id == bookAuthor.Id && (author.FirstName != bookAuthor.FirstName || author.LastName != bookAuthor.LastName || author.Email != bookAuthor.Email || author.Age != bookAuthor.Age))
                    {
                        listOfAuthorsToUpdate.Add(author);
                    }
                }
            }
            if (listOfAuthorsToUpdate.Any())
            {
                Console.WriteLine("----------------================= listOfAuthorsToUpdate ");

                await _Context.Author.BulkUpdateAsync(listOfAuthorsToUpdate);
            }


            //originalAuthors.Where(c => !bookAuthors.Any(b => b.CusId == c.Id));
            var query = originalAuthors
                .Where(c => !bookAuthors
                    .Select(b => b.Id)
                    .Contains(c.Id)
                );
            listOfAuthorsToAdd.AddRange(query);

            if (listOfAuthorsToAdd.Any())
            {
                Console.WriteLine("----------------================= listOfAuthorsToAdd ");

                await _Context.Author.BulkInsertAsync(listOfAuthorsToAdd);
            }
            var listOfAuthorsToDelete = bookAuthors.Where(bookA => !originalAuthors.Select(originalA => originalA.Id).Contains(bookA.Id));
            // delete query
            if (listOfAuthorsToDelete.Any())
            {
                Console.WriteLine("----------------================= listOfAuthorsToDelete ");

                await _Context.Author.BulkDeleteAsync(listOfAuthorsToDelete);
            }
            // TODO: check if bookPulbisher is empty -> count =0 -> listToAdd = originalPublisher
            // TODO: update status
            // TODO: remove repo injection and replace it with dbContext.


        }

    }
}

