using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using IModel = RabbitMQ.Client.IModel;

namespace OT.Assessment.Consumer.Interfaces
{
    public interface IRabbitMqClient
    {
        IModel CreateChannel();
        string QueueName {get;}
    }
}