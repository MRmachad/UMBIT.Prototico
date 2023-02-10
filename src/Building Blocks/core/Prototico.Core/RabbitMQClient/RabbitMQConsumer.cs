using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text.Json;
using UMBIT.Core.Mediator;
using UMBIT.Core.Messages;
using UMBIT.Core.RabbitMQClient.BasicConfig;
using UMBIT.Core.RabbitMQClient.Interfaces;

namespace UMBIT.Core.RabbitMQClient
{
    public class RabbitMQConsumer : IMessageConsumer
    {

        private readonly IMediatorHandler Mediator;

        public RabbitMQConsumer(IMediatorHandler mediator)
        {

            Mediator = mediator;
        }
        public bool ReceiveMessage<T1, T2>(T2 command, CredentialServerRMQ credentialServerRMQ) where T1 : class where T2 : CommandReceiveMessage<T1>
        {
            try
            {
#if DEBUG
                var factory = new ConnectionFactory
                {
                    HostName = credentialServerRMQ.HostName
                };
#else

               var factory = new ConnectionFactory
                {
                    HostName = credentialServerRMQ.HostName,
                    Password = credentialServerRMQ.Senha,
                    UserName = credentialServerRMQ.Login
                };
#endif


                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        if (channel.IsOpen)
                        {
                            var consumer = new EventingBasicConsumer(channel);
                            consumer.Received += (model, eventArgs) =>
                            {
                                var body = eventArgs.Body.ToArray();

                                command.messageRMQ = JsonSerializer.Deserialize<T1>(body);

                                this.Mediator.EnviarComando(command);

                            };

                            return true;
                        }
                        return false;
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Erro no recebimento da mensagem RabbitMQ", e);
            }


        }

        public bool ReceiveMessage<T1,T2>(QueueDeclare queueDeclare, T2 command, CredentialServerRMQ credentialServerRMQ) where T1 : class where T2 : CommandReceiveMessage<T1>
        {
            try
            {
#if DEBUG
                var factory = new ConnectionFactory
                {
                    HostName = credentialServerRMQ.HostName
                };
#else

               var factory = new ConnectionFactory
                {
                    HostName = credentialServerRMQ.HostName,
                    Password = credentialServerRMQ.Senha,
                    UserName = credentialServerRMQ.Login
                };
#endif


                using (var connection = factory.CreateConnection())
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
                          
                                command.messageRMQ = JsonSerializer.Deserialize<T1>(body);

                                this.Mediator.EnviarComando(command);

                            };

                            return true;
                        }

                        return false;

                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Erro no recebimento da mensagem RabbitMQ", e);
            }

        }
    }

    public abstract class CommandReceiveMessage<T> : Command where T : class
    {
        public abstract T messageRMQ { get; set; }
    }
}
