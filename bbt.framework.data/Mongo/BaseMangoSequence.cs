using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.data
{
    public class BaseMangoSequence
    {
        [BsonId]
        public ObjectId _Id { get; set; }

        public string Name { get; set; }

        public long Value { get; set; }

        //public void Insert(IMongoDatabase database)
        //{
        //    var collection = database.GetCollection<BaseMangoSequence>("sequence");
        //    collection.InsertOne(this);
        //}

        //public long GetNextSequenceValue(string sequenceName, IMongoDatabase database)
        //{
        //    var collection = database.GetCollection<BaseMangoSequence>("sequence");
        //    var filter = Builders<BaseMangoSequence>.Filter.Eq(a => a.Name, sequenceName);
        //    var update = Builders<BaseMangoSequence>.Update.Inc(a => a.Value, 1);
        //    var sequence = collection.FindOneAndUpdate(filter, update);

        //    return sequence.Value;
        //}
    }
}
