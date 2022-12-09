using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbt.framework.data
{
    public record BaseDBModel
    {
        [BsonIgnore]
        public virtual long Id { get; set; }
    }
}
