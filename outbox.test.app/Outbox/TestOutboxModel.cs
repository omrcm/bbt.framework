using bbt.framework.outbox;
using data.test.app.Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace outbox.test.app.Outbox
{
    public record TestOutboxModel : BaseOutboxModel<TestModel>
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public override long Id { get; set; }
    }
}
