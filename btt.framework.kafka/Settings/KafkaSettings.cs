using Confluent.Kafka;

namespace bbt.framework.kafka
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }

        public string [] Topic { get; set; }

        public string SslCaLocation { get; set; }

        public SecurityProtocol SecurityProtocol { get; set; } = Confluent.Kafka.SecurityProtocol.Ssl;

        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;
    }
}
