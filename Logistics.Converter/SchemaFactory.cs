using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter
{
    class SchemaFactory
    {
        private static readonly Dictionary<int, Type> SchemaTypeByFieldCount = new Dictionary<int, Type>()
        {
            { 10, typeof(Shipment.Schema)},
            { 21, typeof(Receipt.Schema)},
            { 56, typeof(Stock.Schema)},
        };
        private static Encoding DefaultEncoding = Encoding.GetEncoding("SHIFT_JIS");

        public static AbstractSchema Create(string path)
        {
            using (var sr = new StreamReader(path, DefaultEncoding))
            {
                string line = sr.ReadLine();
                var fields = line.Split(new string[] { "," }, StringSplitOptions.None);
                var type = SchemaTypeByFieldCount[fields.Length];
                return (AbstractSchema)Activator.CreateInstance(type, path);
            }
        }
    }
}
