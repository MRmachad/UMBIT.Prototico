namespace UMBIT.Core.RabbitMQClient
{
    public class RabbitMQConfigs
    {
        public string hostname { get; set; }
        public string user { get; set; }
        public string senha { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public bool Mandatory { get; set; }
        public string Queue { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
    }
}