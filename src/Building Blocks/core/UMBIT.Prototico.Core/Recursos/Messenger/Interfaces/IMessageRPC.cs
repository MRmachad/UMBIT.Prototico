using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentValidation.Results;
using RabbitMQ.Client;

namespace UMBIT.Prototico.Core.Recursos.Messenger.Interfaces
{
    public interface IMessageRPC
    {
        void CreateRPCListener(
                                IModel model,
                                string listener,
                                ConcurrentDictionary<string, TaskCompletionSource<string>> _activeTaskQueue);

        void RequestAsync<TRequest>(
                                      IModel model,
                                      TRequest request,
                                      string queueToSend,
                                      string queueToReply,
                                      string messageIdentification)
                                      where TRequest : new();

        void RespondAsync<TRequest, TResponse>(
                                      IModel model,
                                      string queueToListen,
                                      Func<TRequest, Task<TResponse>> responder)
                                      where TRequest : new()
                                      where TResponse : new();

        void RespondAsync<TRequest>(
                                        IModel model,
                                        string queueToListen,
                                        Func<TRequest, Task<ValidationResult>> responder)
                                        where TRequest : new();
    }
}
