using System;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UMBIT.Core.Mediator;
using UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient.BasicConfig;
using UMBIT.Prototico.Core.Recursos.Messenger.Interfaces;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient
{
    public class RabbitMQConsumer : IMessageConsumer
    {

        public RabbitMQConsumer() {  }
        public void ReceiveMessage<T>(ConnectionFactory factoryConnection, Action<T> action) where T : class
        {
            try
            {
                using (var connection = factoryConnection.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        if (channel.IsOpen)
                        {
                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, eventArgs) =>
                            {
                                var body = eventArgs.Body.ToArray();

                                var res = JsonSerializer.Deserialize<T>(body);

                                action(res);

                            };

                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Erro no recebimento da mensagem RabbitMQ", e);
            }


        }

        public void ReceiveMessageByType<T>(ConnectionFactory factoryConnection, Action<T> action) where T : class
        {
            try
            {
                using (var connection = factoryConnection.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        if (channel.IsOpen)
                        {
                            channel.QueueDeclare(
                             queue: typeof(T).Name,
                             durable: true,
                             exclusive: false,
                             autoDelete: false);

                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, eventArgs) =>
                            {
                                var body = eventArgs.Body.ToArray();

                                var res = JsonSerializer.Deserialize<T>(body);

                                action(res);

                            };

                        }

                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Erro no recebimento da mensagem RabbitMQ", e);
            }
        }
        public void ReceiveMessage<T>(QueueDeclare queueDeclare, ConnectionFactory factoryConnection, Action<T> action) where T : class
        {
            try
            {
                using (var connection = factoryConnection.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        if (channel.IsOpen)
                        {
                            channel.QueueDeclare(
                             queue: queueDeclare.Queue,
                             durable: queueDeclare.Durable,
                             exclusive: queueDeclare.Exclusive,
                             autoDelete: queueDeclare.AutoDelete,
                             arguments: queueDeclare.Arguments);

                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, eventArgs) =>
                            {
                                var body = eventArgs.Body.ToArray();

                                var res = JsonSerializer.Deserialize<T>(body);

                                action(res);

                            };

                        }

                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Erro no recebimento da mensagem RabbitMQ", e);
            }
        }


    }

}
