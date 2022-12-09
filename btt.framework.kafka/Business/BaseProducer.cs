using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace bbt.framework.kafka
{
    public class BaseProducer<T> : IDisposable
    {
        KafkaSettings kafkaSettings;
        IProducer<Null, string> producer;

        ILogger logger;

        JsonSerializerSettings jsonSettings = null;
        public BaseProducer(KafkaSettings _kafkaSettings, ILogger _logger)
        {
            kafkaSettings = _kafkaSettings;
            logger = _logger;

            jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };


            Connect();
        }
        private void Connect()
        {
            try
            {
                producer = new ProducerBuilder<Null, string>(GetProducerConfig())
                                                         .Build();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }

        ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                SecurityProtocol = kafkaSettings.SecurityProtocol,
                SslCaLocation = kafkaSettings.SslCaLocation
            };
        }
        public async Task Send(string topic , T data)
        {
            try
            {
                string model = JsonConvert.SerializeObject(data, jsonSettings);
                await producer.ProduceAsync(topic, new Message<Null, string> { Value = model });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
        public async Task Send(T data)
        {
            await Send(kafkaSettings.Topic[0], data);
        }

        public void Dispose()
        {
            producer.Dispose();
        }
    }
}
