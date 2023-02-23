using RabbitMQ.Client;
using System.Collections.Generic;

namespace UMBIT.Core.RabbitMQClient.BasicConfig
{
    public class BasicPublish
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public bool Mandatory { get; set; }
        public IBasicProperties BasicProperties { get; set; }
        public BasicPublish(string exchange, string routingKey, bool mandatory = false, IBasicProperties basicProperties = null)
        {
            Exchange = exchange;
            Mandatory = mandatory;
            RoutingKey = routingKey;
            BasicProperties = basicProperties;
        }
    }

    public class QueueDeclare
    {
        public string Queue { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public IDictionary<string, object> Arguments { get; set; }

        public QueueDeclare(string queue = "", bool durable = true, bool exclusive = false,
        bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            Queue = queue;
            Durable = durable;
            Exclusive = exclusive;
            AutoDelete = autoDelete;
            Arguments = arguments;
        }
    }

    public class CredentialServerRMQ
    {
        public string HostName { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }

        public CredentialServerRMQ(string login, string senha, string hostName = "localhost")
        {
            this.Login = login;
            this.Senha = senha;
            this.HostName = hostName;
        }
    }

}
