using FluentValidation.Results;
using MediatR;
using System.Threading.Tasks;
using UMBIT.Core.Messages;
using UMBIT.Core.RabbitMQClient.Interfaces;
using UMBIT.Core.RabbitMQClient.BasicConfig;
using Microsoft.Extensions.Options;
using System;
using UMBIT.Core.RabbitMQClient;

namespace UMBIT.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IMessageProducer _messageProducerMQ;
        private IOptions<RabbitMQConfigs> _appServiceSettings;

        public MediatorHandler(IMediator mediator, IMessageProducer _messageProducer, IOptions<RabbitMQConfigs> options)
        {
            _mediator = mediator;
            _messageProducerMQ = _messageProducer;
            _appServiceSettings = options;
        }

        public async Task<ValidationResult> EnviarComando<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }

        public async Task<TRes> EnviarComando<T, TRes>(T comando)
            where T : Command<TRes>
            where TRes : CommandResponse
        {
            return await _mediator.Send(comando);
        }

        public Task EnviaMensageRabbitMQ<T>(T evento) where T : class
        {
            var res = _messageProducerMQ.SendMessage<T>(
                                               evento,
                                               new QueueDeclare(
                                                   this._appServiceSettings.Value.Queue == null ? String.Empty : this._appServiceSettings.Value.Queue,
                                                   this._appServiceSettings.Value.Durable,
                                                   this._appServiceSettings.Value.Exclusive,
                                                   this._appServiceSettings.Value.AutoDelete
                                               ),
                                               new BasicPublish(
                                                   this._appServiceSettings.Value.Exchange == null ? String.Empty : this._appServiceSettings.Value.Exchange,
                                                   this._appServiceSettings.Value.RoutingKey,
                                                   this._appServiceSettings.Value.Mandatory
                                               ),
                                               new CredentialServerRMQ(
                                                    this._appServiceSettings.Value.user == null ? String.Empty : this._appServiceSettings.Value.user,
                                                    this._appServiceSettings.Value.senha == null ? String.Empty : this._appServiceSettings.Value.senha,
                                                    this._appServiceSettings.Value.hostname == null ? "localhost" : this._appServiceSettings.Value.hostname
                                                   )
                                             );

            return Task.FromResult(res);

        }
    }
}
