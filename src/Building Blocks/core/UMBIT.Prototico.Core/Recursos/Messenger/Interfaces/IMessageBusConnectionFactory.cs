using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Interfaces
{
    public interface IMessageBusConnectionFactory
    {
        IConnection MessageBusConnectionFactory();

    }
}