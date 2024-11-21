using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OT.Assessment.Consumer.Interfaces;
using RabbitMQ.Client;

namespace OT.Assessment.Consumer.Messaging
{
    public class RabbitMqClient : IRabbitMqClient, IDisposable
    {
          private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;
        private IModel _channel;
        private bool _disposed;

        public RabbitMqClient(IConfiguration configuration)
        {
            _hostname = configuration["RabbitMq:HostName"];
            _queueName = configuration["RabbitMq:QueueName"];
            _userName = configuration["RabbitMq:Username"];
            _password = configuration["RabbitMq:Password"];

            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _userName,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable : false, exclusive : false, autoDelete: false, arguments: null);
        }


        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }

        public string QueueName => _queueName;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            {
                if(!disposing)
                {
                    _channel?.Close();
                    _channel?.Close();
                    _connection?.Close();
                    _connection?.Close();
                }
                _disposed = true;
            }
        }
    }      
}