using BookAPI.Data;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Managers.Sender
{
    public class AuthorUpdateSender:IAuthorUpdateSender
    {
        private IConnection _connection;

        public void SendAuthor(Author author)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "author", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(author);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: "author", basicProperties: null, body: body);
                }

            }

        }
        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }
        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}


//var factory = new ConnectionFactory() { HostName = "localhost" };
//using (var connection = factory.CreateConnection())
//{
//    using (var channel = connection.CreateModel())
//    {
//        channel.QueueDeclare(queue: "counter",
//                     durable: true,
//                     exclusive: false,
//                     autoDelete: false,
//                     arguments: null);

//        var message = $"Message {_messageCount++}";

//        Dictionary<string, int> messages = null;
//        _memoryCache.TryGetValue<Dictionary<string, int>>("messages", out messages);
//        if (messages == null) messages = new Dictionary<string, int>();
//        messages.Add(message, _messageCount);
//        _memoryCache.Set<Dictionary<string, int>>("messages", messages);

//        var messageBody = Encoding.UTF8.GetBytes(message);

//        channel.BasicPublish(exchange: "counter", routingKey: "counter", body: messageBody, basicProperties: null);
//    }


