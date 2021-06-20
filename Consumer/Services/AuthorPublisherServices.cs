using BookAPI.Data;
using BookAPI.Repositories.Interfaces;
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
        public AuthorPublisherServices(HttpClient httpClient, IRepository<Author, int> authorRepository, IRepository<Publisher, int> publisherRepository)
        {
            _httpClient = httpClient;
            _authorRepository = authorRepository;
            _publisherRepository = publisherRepository;
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
    }
}
