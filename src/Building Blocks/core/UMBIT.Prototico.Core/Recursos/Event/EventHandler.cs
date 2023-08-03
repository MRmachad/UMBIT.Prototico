using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace UMBIT.Prototico.Core.Recursos.Event
{
    public abstract class EventHandler<T> : INotificationHandler<T> where T : Event
    {
        public abstract Task Handle(T notification, CancellationToken cancellationToken);
    }
}
