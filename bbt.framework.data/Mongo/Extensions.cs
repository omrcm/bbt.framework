using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.data
{
    public static class Extensions
    {
        public static long GetNextSequenceValue(this IMongoDatabase database,string sequenceName)
        {
            var collection = database.GetCollection<BaseMangoSequence>("sequence");
            var filter = Builders<BaseMangoSequence>.Filter.Eq(a => a.Name, sequenceName);
            var update = Builders<BaseMangoSequence>.Update.Inc(a => a.Value, 1);
            var sequence = collection.FindOneAndUpdate(filter, update, new FindOneAndUpdateOptions<BaseMangoSequence> { IsUpsert = true,ReturnDocument = ReturnDocument.After });


            return sequence.Value;
        }
    }
}
