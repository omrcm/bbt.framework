using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace kafka.ksqldb.test.app.Business
{
    public class DataTypeConverter
    {
        public string GetType(PropertyInfo propertyInfo)
        {
            KSQLTypeAttribute? kSQLTypeAttribute = propertyInfo.GetCustomAttribute<KSQLTypeAttribute>();
            string dataType = string.Empty;
            if (kSQLTypeAttribute != null)
            {
                dataType = kSQLTypeAttribute.KSQLType;
            }
            else
            {
                if (
                    propertyInfo.PropertyType.Equals(typeof(bool)) ||
                    propertyInfo.PropertyType.Equals(typeof(Boolean))
                    )
                {
                    dataType = "BOOLEAN";
                }
                else if (
    propertyInfo.PropertyType.Equals(typeof(int)) ||
    propertyInfo.PropertyType.Equals(typeof(Int16)) ||
    propertyInfo.PropertyType.Equals(typeof(Int32))
    )
                {
                    dataType = "INTEGER";
                }
                else if (
propertyInfo.PropertyType.Equals(typeof(long)) ||
propertyInfo.PropertyType.Equals(typeof(Int64))
)
                {
                    dataType = "BIGINT";
                }
                else if (
propertyInfo.PropertyType.Equals(typeof(double)) ||
propertyInfo.PropertyType.Equals(typeof(Double)) ||
propertyInfo.PropertyType.Equals(typeof(decimal)) ||
propertyInfo.PropertyType.Equals(typeof(Decimal)) ||
propertyInfo.PropertyType.Equals(typeof(float))
)
                {
                    dataType = "DOUBLE";
                }
                else
                {
                    dataType = "VARCHAR";
                }

            }
            return dataType;
        }
    }
}
