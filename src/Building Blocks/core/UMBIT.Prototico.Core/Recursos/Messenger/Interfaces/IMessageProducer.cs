using UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient.BasicConfig;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Interfaces
{
    public interface IMessageProducer
    {
        bool SendMessage<T>(T message, BasicPublish basicPublish, CredentialServerRMQ credentialServerRMQ);
        bool SendMessage<T>(T message, QueueDeclare queueDeclare, BasicPublish basicPublish, CredentialServerRMQ credentialServerRMQ);
    }
}
