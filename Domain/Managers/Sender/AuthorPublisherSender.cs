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
    public class AuthorPublisherSender : ISender
    {
        private IConnection _connection;

        public void SendAuthorPublisher(toSend authorObj)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "authorPublisher", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    
                    var json = JsonConvert.SerializeObject(authorObj);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: "authorPublisher", basicProperties: null, body: body);
                }

            }

        }
        //public void SendPublisher(toSend publisherObj)
        //{
        //    if (ConnectionExists())
        //    {
        //        using (var channel = _connection.CreateModel())
        //        {
        //            channel.QueueDeclare(queue: "publisher", durable: true, exclusive: false, autoDelete: false, arguments: null);

        //            var json = JsonConvert.SerializeObject(publisherObj);
        //            var body = Encoding.UTF8.GetBytes(json);

        //            channel.BasicPublish(exchange: "", routingKey: "publisher", basicProperties: null, body: body);
        //        }

        //    }
        //}
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