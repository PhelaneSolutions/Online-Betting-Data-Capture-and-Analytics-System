using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OT.Assessment.Consumer.Db;
using OT.Assessment.Consumer.Interfaces;
using OT.Assessment.Consumer.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OT.Assessment.Consumer.Service
{
    public class ConsumeService : BackgroundService
    {
        private readonly ILogger<ConsumeService> _logger;
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly ConsumerDb _consumerDb;
        private IModel _channel;
        private IServiceScopeFactory _serviceScopeFactory;

        public ConsumeService(ILogger<ConsumeService> logger,IRabbitMqClient rabbitMqClient, ConsumerDb db, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _rabbitMqClient = rabbitMqClient;
            _consumerDb = db;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel = _rabbitMqClient.CreateChannel();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var wager = JsonSerializer.Deserialize<CasinoWager>(message);

                if(wager != null)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ConsumerDb>();
                        dbContext.casinoWagers.Add(wager);
                        await dbContext.SaveChangesAsync();
                    }
                }
            };
            _channel.BasicConsume(queue: _rabbitMqClient.QueueName, autoAck : false, consumer: consumer);

            return Task.CompletedTask; 
        }

        public override void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            base.Dispose();
        }
    }
}