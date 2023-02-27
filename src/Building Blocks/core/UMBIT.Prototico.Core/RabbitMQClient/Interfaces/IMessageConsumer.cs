using UMBIT.Core.RabbitMQClient.BasicConfig;

namespace UMBIT.Core.RabbitMQClient.Interfaces
{
    public interface IMessageConsumer
    {
        bool ReceiveMessage<T1, T2>(T2 command, CredentialServerRMQ credentialServerRMQ) where T1 : class where T2 : CommandReceiveMessage<T1>;
        bool ReceiveMessage<T1, T2>(QueueDeclare queueDeclare, T2 command, CredentialServerRMQ credentialServerRMQ) where T1 : class where T2 : CommandReceiveMessage<T1>;
    }
}
