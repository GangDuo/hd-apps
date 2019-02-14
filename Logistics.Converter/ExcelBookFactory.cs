using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter
{
    class ExcelBookFactory
    {
        private static readonly Dictionary<Type, Type> ExcelBookTypeBySchema = new Dictionary<Type, Type>()
        {
            { typeof(Shipment.Schema), typeof(Shipment.ExcelBook)},
            { typeof(Receipt.Schema), typeof(Receipt.ExcelBook)},
            { typeof(Stock.Schema), typeof(Stock.ExcelBook)},
        };

        public static AbstractExcelBook CreateFrom(AbstractSchema schema)
        {
            var type = ExcelBookTypeBySchema[schema.GetType()];
            return (AbstractExcelBook)Activator.CreateInstance(type, schema);
        }
    }
}
