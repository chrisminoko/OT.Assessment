using OT.Assessment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Services.Producer
{
    public interface IMessageProducer
    {
        Task SendMessage<T>(T message, EventQueue QueueName);
    }
}
