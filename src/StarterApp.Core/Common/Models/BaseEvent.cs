using MediatR;
using System;
namespace StarterApp.Core.Common.Models
{
    public abstract class BaseEvent : IRequest<bool>, INotification
    {
        public DateTime Timestamp { get; private set; }
        public string MessageType { get; protected set; }
        protected BaseEvent()
        {
            Timestamp = DateTime.Now;
            MessageType = GetType().Name;
        }
    }
}
