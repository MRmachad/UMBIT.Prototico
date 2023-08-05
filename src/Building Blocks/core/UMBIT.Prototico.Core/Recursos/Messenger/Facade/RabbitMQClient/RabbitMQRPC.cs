using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation.Results;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UMBIT.Prototico.Core.Recursos.Messenger.Interfaces;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Facade.RabbitMQClient
{
    public class RabbitMQRPC : IMessageRPC
    {

        public void CreateRPCListener(
            IModel model,
            string listener,
            ConcurrentDictionary<string, TaskCompletionSource<string>> _activeTaskQueue)
        {

            var consumer = new EventingBasicConsumer(model);
            model.QueueDeclare(listener, durable: false, false, false, null);
            consumer.Received += (model, ea) =>
            {
                if (!_activeTaskQueue.TryRemove(ea.BasicProperties.CorrelationId, out var tcs)) return;

                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };

            model.BasicConsume(
                                     consumer: consumer,
                                     queue: listener,
                                     autoAck: true);
        }

        public void RequestAsync<TRequest>(
                                            IModel model,
                                            TRequest request,
                                            string queueToSend,
                                            string queueToReply,
                                            string messageIdentification)
                                            where TRequest : new()
        {
            var messageToSend = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request, request.GetType()));
            var basicProperties = model.CreateBasicProperties();
            basicProperties.CorrelationId = messageIdentification;
            basicProperties.ReplyTo = queueToReply;

            model.QueueDeclare(
                                queueToSend,
                                 false,
                                 false,
                                 false,
                                 null);

            model.BasicPublish(exchange: String.Empty, routingKey: queueToSend, basicProperties: basicProperties, body: messageToSend);

        }

        public void RespondAsync<TRequest, TResponse>(
                                                    IModel model,
                                                    string queueToListen,
                                                    Func<TRequest, Task<TResponse>> responder)
                                                    where TRequest : new()
                                                    where TResponse : new()
        {

            model.QueueDeclare(queueToListen, durable: false, false, false, null);

            var consumer = new EventingBasicConsumer(model);

            string response = string.Empty;

            consumer.Received += async (bc, ea) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var data = JsonSerializer.Deserialize<TRequest>(message);
                    var fromResponder = await responder(data);
                    response = JsonSerializer.Serialize(fromResponder, fromResponder.GetType());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {

                    var responseMessage = Encoding.UTF8.GetBytes(response);
                    var basicProperties = model.CreateBasicProperties();
                    basicProperties.CorrelationId = ea.BasicProperties.CorrelationId;

                    model.BasicPublish(exchange: string.Empty,
                                         routingKey: ea.BasicProperties.ReplyTo,
                                         basicProperties: basicProperties,
                                         body: responseMessage);

                }
            };

            model.BasicConsume(queue:queueToListen, true, consumer);

        }
        public void RespondAsync<TRequest>(
                                        IModel model,
                                        string queueToListen,
                                        Func<TRequest, Task<ValidationResult>> responder)
                                        where TRequest : new()
        {

            model.QueueDeclare(queueToListen, durable: false, false, false, null);

            var consumer = new EventingBasicConsumer(model);

            string response = string.Empty;

            consumer.Received += async (bc, ea) =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var data = JsonSerializer.Deserialize<TRequest>(message);
                    var fromResponder = await responder(data);
                    response = JsonSerializer.Serialize(fromResponder, fromResponder.GetType());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {

                    var responseMessage = Encoding.UTF8.GetBytes(response);
                    var basicProperties = model.CreateBasicProperties();
                    basicProperties.CorrelationId = ea.BasicProperties.CorrelationId;

                    model.BasicPublish(exchange: string.Empty,
                                         routingKey: ea.BasicProperties.ReplyTo,
                                         basicProperties: basicProperties,
                                         body: responseMessage);

                }
            };

            model.BasicConsume(queue: queueToListen, true, consumer);
        }

    }
}
