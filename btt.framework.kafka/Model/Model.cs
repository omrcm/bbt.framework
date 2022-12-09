using System;

namespace bbt.framework.kafka
{
    public class Headers
    {
        public string operation { get; set; }
        public string changeSequence { get; set; }
        public DateTime? timestamp { get; set; }
        public string streamPosition { get; set; }
        public string transactionId { get; set; }
        public string changeMask { get; set; }
        public string columnMask { get; set; }
        public int? transactionEventCounter { get; set; }
        public bool? transactionLastEvent { get; set; }
    }

    public class Message<T>
    {
        public T data { get; set; }
        public T beforeData { get; set; }
        public Headers headers { get; set; }
    }

    public class KafkaModel<T>

    {
        public string magic { get; set; }
        public string type { get; set; }
        public object headers { get; set; }
        public object messageSchemaId { get; set; }
        public object messageSchema { get; set; }
        public Message<T> message { get; set; }
    }
}
