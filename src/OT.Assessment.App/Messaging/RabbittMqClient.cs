using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OT.Assessment.App.Interfaces.RabbitMQ;
using RabbitMQ.Client;

namespace OT.Assessment.App.Messaging
{
    public class RabbittMqClient : IPublishWagerAsync, IDisposable
    {
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;
        private IModel _channel;
        private bool _disposed;
        private ILogger<RabbittMqClient> _logger;

        public RabbittMqClient(IConfiguration configuration, ILogger<RabbittMqClient> logger)
        {
            _hostname = configuration["RabbitMq:HostName"];
            _queueName = configuration["RabbitMq:QueueName"];
            _userName = configuration["RabbitMq:Username"];
            _password = configuration["RabbitMq:Password"];
            _logger = logger;
            
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


        public  Task PublishWagerAsync<T>(T message)
        {
          var messageBody = JsonSerializer.Serialize(message);
          var body = Encoding.UTF8.GetBytes(messageBody);

          _channel.BasicPublish(exchange : "", routingKey :_queueName, basicProperties: null, body:body);
          _logger.LogInformation("Message published to RabbitMQ: {0}",messageBody);
          return Task.CompletedTask;
        }

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
                    _connection?.Close();
                }
                _disposed = true;
            }
        }
    }
}