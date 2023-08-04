using RabbitMQ.Client;
using UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient.BasicConfig;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Interfaces
{
    public interface IMessageProducer
    {
        void SendMessage<T>(IModel model, T message) where T : class;
        void SendMessageByType<T>(IModel model, T message) where T : class;
    }
}
