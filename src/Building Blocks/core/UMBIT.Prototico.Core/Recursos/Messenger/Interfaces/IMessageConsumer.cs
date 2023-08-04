using RabbitMQ.Client;
using System;
using UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient.BasicConfig;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Interfaces
{
    public interface IMessageConsumer
    {
        void ReceiveMessage<T>(IModel model , Action<T> action) where T : class;
        void ReceiveMessageByType<T>(IModel model, Action<T> action) where T : class;
    }
}
