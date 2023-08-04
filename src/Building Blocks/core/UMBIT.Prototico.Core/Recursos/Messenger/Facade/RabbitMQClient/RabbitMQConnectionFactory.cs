using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using UMBIT.Prototico.Core.Recursos.Messenger.Interfaces;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient
{
    internal class RabbitMQConnectionFactory : IMessageBusConnectionFactory
    {
        public IConnection ConnectionFactory { get; private set; }

        private RabbitMQConnectionFactory(IOptions<RabbitMQConfigs> options)
        {
            var ConnectionFactory = new ConnectionFactory()
            {
                Port = options.Value.Port,
                UserName = options.Value.User,
                Password = options.Value.Senha,
                HostName = options.Value.Hostname,
            };

            ConnectionFactory.AutomaticRecoveryEnabled = true;
            ConnectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
            ConnectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(60);

            this.ConnectionFactory = ConnectionFactory?.CreateConnection();
        }

        public IConnection MessageBusConnectionFactory()
        {
            return this.ConnectionFactory;
        }
    }

}