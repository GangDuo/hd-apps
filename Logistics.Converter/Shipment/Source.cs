using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Shipment
{
    class Source
    {
        // 指示数
        [Index(1)]
        [TypeConverter(typeof(Int32Converter))]
        public int ExpectedQty { get; set; }

        // 納入先コード
        [Index(2)]
        public string DeliveryDestinationCode { get; set; }

        [Index(4)]
        public string JanCode { get; set; }

        // 出荷数
        [Index(5)]
        [TypeConverter(typeof(Int32Converter))]
        public int ActualQty { get; set; }

        // 出荷日        
        [Index(6)]
        [TypeConverter(typeof(DateTimeConverter))]
        [DateTimeStyles(System.Globalization.DateTimeStyles.RoundtripKind)]
        //[Format("yyyy-MM-dd")]
        public DateTime DeliveredAt { get; set; }

        [Index(8)]
        public string PurchaseOrderNumber { get; set; }

        [Index(9)]
        [TypeConverter(typeof(BooleanConverter))]        
        public bool IsReservedItem { get; set; }
    }
}
