using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Receipt
{
    /**
     */
    class Source
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string JanCode { get; set; }
        public string SupplyChainManagementCode { get; set; }
        public int Qty { get; set; }
        public int UnitCost { get; set; }
        public int Cost { get; set; }
        public int UnitPrice { get; set; }
        public int Price { get; set; }
        public string VarietyCode { get; set; }
    }
}
