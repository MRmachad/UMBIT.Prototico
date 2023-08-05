using System;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UMBIT.Prototico.Core.Recursos.Messenger.Interfaces;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using FluentValidation.Results;
using System.Collections.Generic;

namespace UMBIT.Prototico.Core.Recursos.Messenger
{
    public class MessengerBus
    {
        protected IModel Channel { get; set; }
        protected IConnection Connection { get; set; }

        protected readonly IMessageRPC _messageRPC;
        protected readonly IMessageProducer _messageProducer;
        protected readonly IMessageConsumer _messageConsumer;
        protected readonly IMessageBusConnectionFactory _connectionFactory;


        protected List<String> ListenersRPC;
        protected readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _activeTaskQueue = new ConcurrentDictionary<string, TaskCompletionSource<string>>();


        protected bool IsConnected => this.Connection?.IsOpen ?? false;

        public MessengerBus(
            IMessageRPC _messageRPC,
            IMessageProducer messageProducer,
            IMessageConsumer _messageConsumer,
            IMessageBusConnectionFactory connectionFactory)
        {
            this.ListenersRPC = new List<string>();

            this._messageRPC = _messageRPC;
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

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(
            TRequest request)
            where TRequest : new()
            where TResponse : new()
        {
            this.TryConnect();
            this.CreateRPCListener(request.GetType().Name + "_RPC");

            var messageIdentification = Guid.NewGuid().ToString();
            var taskCompletionSource = new TaskCompletionSource<string>();
            this._activeTaskQueue.TryAdd(messageIdentification, taskCompletionSource);

            this._messageRPC.RequestAsync(
                this.Channel,
                request,
                request.GetType().Name,
                request.GetType().Name + "_RPC",
                messageIdentification);

            var response = await taskCompletionSource.Task;
            return JsonSerializer.Deserialize<TResponse>(response);
        }
        public async Task<ValidationResult> RequestAsync<TRequest>(
            TRequest request)
            where TRequest : new()
        {
            this.TryConnect();
            this.CreateRPCListener(request.GetType().Name + "_RPC");

            var messageIdentification = Guid.NewGuid().ToString();
            var taskCompletionSource = new TaskCompletionSource<string>();
            this._activeTaskQueue.TryAdd(messageIdentification, taskCompletionSource);

            this._messageRPC.RequestAsync(
                this.Channel,
                request,
                request.GetType().Name,
                request.GetType().Name + "_RPC",
                messageIdentification);

            var response = await taskCompletionSource.Task;
            return JsonSerializer.Deserialize<ValidationResult>(response);
        }

        public Task RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
            where TRequest : new()
            where TResponse : new()
        {
            TryConnect();

            this._messageRPC.RespondAsync(
                this.Channel,
                typeof(TRequest).Name,
                responder);

            return Task.CompletedTask;
        }
        public Task RespondAsync<TRequest, TResponse>(Func<TRequest, Task<ValidationResult>> responder)
            where TRequest : new()
            where TResponse : new()
        {
            TryConnect();

            this._messageRPC.RespondAsync(
                this.Channel,
                typeof(TRequest).Name,
                responder);

            return Task.CompletedTask;
        }
        private bool TryConnect()
        {

            if (IsConnected) return IsConnected;

            this.Connection = this._connectionFactory.MessageBusConnectionFactory();
            this.Channel = this.Connection.CreateModel();

            this.Connection.ConnectionShutdown += (connection, evt) =>
            {
                //TODO

                //REALIZAR TRATATIVAS DE RECONEXÃO NO RABBITMQ
            };

            return Channel.IsOpen;
        }

        private void CreateRPCListener(string listener)
        {

            if (!this.ListenersRPC.Contains(listener) && this.TryConnect())
            {
                this._messageRPC.CreateRPCListener(
                    this.Channel,
                    listener,
                    _activeTaskQueue
                    );

                this.ListenersRPC.Add(listener);
            }
        }


    }
}
