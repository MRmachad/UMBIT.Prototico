using MediatR;
using System;

namespace UMBIT.Prototico.Core.Recursos.Event
{
    public abstract class Event : Message, INotification
    {
        public DateTime Timestamp { get; private set; }

        public Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
