using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kafka.ksqldb.test.app
{
    public class KSQLTypeAttribute : Attribute
    {
        public string KSQLType { get; set; }
        public KSQLTypeAttribute(string _KSQLType)
        {
            KSQLType = _KSQLType;
        }
    }
}
