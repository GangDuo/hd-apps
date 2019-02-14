using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Stock
{
    class Source
    {
        [Index(7)]
        public string JanCode { get; set; }

        [Index(16)]
        [TypeConverter(typeof(Int32Converter))]
        public int Qty { get; set; }
    }
}
