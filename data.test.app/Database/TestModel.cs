using bbt.framework.data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data.test.app.Database
{
    public record TestModel : BaseDBModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int64)]
        public override long Id { get; set; }

        public string Title { get; set; }
    }
}
