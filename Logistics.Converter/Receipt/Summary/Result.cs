using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Receipt.Summary
{
    class Result
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string SupplyChainManagementCode { get; set; }
        public int TotalQty { get; set; }
        public int TotalCost { get; set; }
        public int TotalPrice { get; set; }
        // 区分
        public string Dmy1 { get; set; }
    }
}
