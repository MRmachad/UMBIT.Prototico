using RabbitMQ.Client;
using System;
using UMBIT.Prototico.Core.Recursos.Messenger.Interfaces;

namespace UMBIT.Prototico.Core.Recursos.Messenger
{
    public class MessengerBus
    {
        protected IModel Channel { get; set; }
        protected IConnection Connection { get; set; }

        protected readonly IMessageProducer _messageProducer;
        protected readonly IMessageConsumer _messageConsumer;
        protected readonly IMessageBusConnectionFactory _connectionFactory;

        protected bool IsConnected => this.Connection?.IsOpen ?? false;

        public MessengerBus(IMessageProducer messageProducer, IMessageConsumer _messageConsumer, IMessageBusConnectionFactory connectionFactory)
        {
            this._messageProducer = messageProducer;
            this._messageConsumer = _messageConsumer;
            this._connectionFactory = connectionFactory;
        }
        public void ReceiveMessage<T>(Action<T> action) where T : class
        {
            this.TryConnect();
            this._messageConsumer.ReceiveMessage(Channel, action);
        }

        public void ReceiveMessageByType<T>(Action<T> action) where T : class
        {
            this.TryConnect();
            this._messageConsumer.ReceiveMessageByType(Channel, action);
        }

        public void SendMessage<T>(T message)
        {
            this.TryConnect();
            this.SendMessage(message);
        }

        public void SendMessageByType<T>(T message)
        {
            this.TryConnect();
            this.SendMessageByType(message);
        }

        private void TryConnect()
        {

            if (IsConnected) return;

            this.Connection = this._connectionFactory.MessageBusConnectionFactory();
            this.Channel = this.Connection.CreateModel();

            this.Connection.ConnectionShutdown += (connection, evt) =>
            {
                //TODO

                //REALIZAR TRATATIVAS DE RECONEXÃO NO RABBITMQ
            };

        }

    }
}
