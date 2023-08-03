using System;

namespace UMBIT.Prototico.Core.Recursos
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid AggregateId { get; protected set; }

        protected Message()
        {
            AggregateId = Guid.NewGuid();
            MessageType = GetType().Name;
        }
    }
}
