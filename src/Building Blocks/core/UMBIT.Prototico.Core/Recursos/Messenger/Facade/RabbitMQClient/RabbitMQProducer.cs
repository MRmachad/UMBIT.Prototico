using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient.BasicConfig;
using UMBIT.Prototico.Core.Recursos.Messenger.Interfaces;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient
{
    public class RabbitMQProducer : IMessageProducer
    {
        public void SendMessage<T>(IModel model, T message) where T : class
        {
            var jsonMessage = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            model.BasicPublish(
                exchange: String.Empty,
                String.Empty,
                basicProperties: null,
                body: jsonMessage);
        }
        public void SendMessageByType<T>(IModel model, T message) where T : class
        {
            model.QueueDeclare(message.GetType().Name, durable: true, false, false, null);
            var jsonMessage = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));


            model.BasicPublish(
                exchange: String.Empty,
                routingKey: message.GetType().Name,
                true,
                basicProperties: null,
                body: jsonMessage);
        }
    }
}
