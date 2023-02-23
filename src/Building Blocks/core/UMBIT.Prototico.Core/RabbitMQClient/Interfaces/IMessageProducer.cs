using UMBIT.Core.RabbitMQClient.BasicConfig;

namespace UMBIT.Core.RabbitMQClient.Interfaces
{
    public interface IMessageProducer
    {
        bool SendMessage<T>(T message, BasicPublish basicPublish, CredentialServerRMQ credentialServerRMQ);
        bool SendMessage<T>(T message, QueueDeclare queueDeclare, BasicPublish basicPublish, CredentialServerRMQ credentialServerRMQ);
    }
}
