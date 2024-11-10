using OT.Assessment.Core.Enums;
using OT.Assessment.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Services.Producer
{
    public interface IMessageProducer
    {
        Task<Result> SendMessage<T>(T message, EventQueue QueueName);
    }
}
