using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using UMBIT.Core.RabbitMQClient.BasicConfig;
using UMBIT.Core.RabbitMQClient.Interfaces;

namespace UMBIT.Core.RabbitMQClient
{


    public class RabbitMQProducer : IMessageProducer
    {
        public bool SendMessage<T>(T message, BasicPublish basicPublish, CredentialServerRMQ credentialServerRMQ)
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
                            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));


                            channel.BasicPublish(
                                exchange: basicPublish.Exchange,
                                mandatory: basicPublish.Mandatory,
                                routingKey: basicPublish.RoutingKey,
                                basicProperties: basicPublish.BasicProperties,
                                body: body
                                );

                            return true;
                        }

                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro no envio de mensagem ao RabbitMQ", e);
            }


        }
        public bool SendMessage<T>(T message, QueueDeclare queueDeclare, BasicPublish basicPublish, CredentialServerRMQ credentialServerRMQ)
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
                            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                            channel.QueueDeclare(
                                queue: queueDeclare.Queue,
                                durable: queueDeclare.Durable,
                                exclusive: queueDeclare.Exclusive,
                                autoDelete: queueDeclare.AutoDelete,
                                arguments: queueDeclare.Arguments);

                            channel.BasicPublish(

                                exchange: basicPublish.Exchange,
                                mandatory: basicPublish.Mandatory,
                                routingKey: basicPublish.RoutingKey,
                                basicProperties: basicPublish.BasicProperties,
                                body: body
                                );

                            return true;
                        }

                        return false;

                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Erro no envio de mensagem ao RabbitMQ", e);
            }

        }
    }
}
