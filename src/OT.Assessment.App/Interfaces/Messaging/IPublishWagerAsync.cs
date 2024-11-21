using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OT.Assessment.App.Models;

namespace OT.Assessment.App.Interfaces.RabbitMQ
{
    public interface IPublishWagerAsync
    {
        public  Task PublishWagerAsync<T>(T message);

    }
}