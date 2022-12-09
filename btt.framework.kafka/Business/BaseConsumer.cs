using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace bbt.framework.kafka
{
    public abstract class BaseConsumer<T>
    {
        public Action<T>? OnConsume { get; set; }

        KafkaSettings kafkaSettings;
        CancellationToken cancellationToken;

        ILogger logger;
        JsonSerializerSettings jsonSettings = null;

        public BaseConsumer(KafkaSettings _kafkaSettings, CancellationToken _cancellationToken, ILogger _logger)
        {
            kafkaSettings = _kafkaSettings;
            cancellationToken = _cancellationToken;
            logger = _logger;

            jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        ConsumerConfig GetConsumerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                GroupId = kafkaSettings.GroupId,
                SecurityProtocol = kafkaSettings.SecurityProtocol,
                SslCaLocation = kafkaSettings.SslCaLocation,
                AutoOffsetReset = kafkaSettings.AutoOffsetReset,                
                EnableAutoCommit = true

            };
        }

        public async Task ConsumeAsync()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(GetConsumerConfig())
                .Build())
            {
                consumer.Subscribe(kafkaSettings.Topic);

                Console.WriteLine($"Subscribed to {kafkaSettings.Topic}");


                await Task.Run(async () =>
                {
                    bool taskIsRunning = true;
                    cancellationToken.Register(() =>
                    {
                        taskIsRunning = false;
                    });

      
                    while (taskIsRunning)
                    {
                        string message = string.Empty;

                        try
                        {                         
                            var consumeResult = consumer.Consume(cancellationToken);

                            if (consumeResult.Message is Message<Ignore, string> result)
                            {

                                bool IsIgnore = false;
                                bool IsSuccess = true;

                                T model;
                                if (typeof(T).Equals(typeof(string)))
                                {
                                    model = (T)Convert.ChangeType(result.Value, typeof(T));
                                }
                                else
                                {
                                    message = result.Value;
                                    model = JsonConvert.DeserializeObject<T>(result.Value, jsonSettings);
                                    
                                    if (model.GetType().IsGenericType && typeof(KafkaModel<>) == model.GetType().GetGenericTypeDefinition())
                                    {

                                        JObject jsonObject = JObject.Parse(result.Value);

                                        JToken data = jsonObject.SelectToken("message.data");
                                        JToken beforeData = jsonObject.SelectToken("message.beforeData");

                                        if (data.Equals(beforeData))
                                        {
                                            IsIgnore = true;
                                        }
                                    }
                                }

                                if (!IsIgnore)
                                {
                                    IsSuccess = await Process(model);
                                    OnConsume?.Invoke(model);
                                }

                                //if (IsSuccess)
                                //{
                                //    consumer.Commit(consumeResult);
                                //}

                            }
                        }
                        catch (Exception ex)
                        {
                            

                            logger.LogError(message);
                            logger.LogError(ex.ToString());
                            
                        }
                    }
                });

                consumer.Close();
            }
        }

        public abstract Task<bool> Process(T model);
    }
}
