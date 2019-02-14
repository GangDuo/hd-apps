using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Receipt
{
    class Result
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public DateTime DeliveredAt { get; set; }
        public string SupplyChainManagementCode { get; set; }
        public string VarietyCode { get; set; }
        public string SupplierCode { get; set; }
        public string JanCode { get; set; }
        public string ModelNo { get; set; }
        public string ProductName { get; set; }
        //public DateTime PurchasedAt { get; set; }
        public string PurchasedAt { get; set; }
        public int Qty { get; set; }
        public int UnitCost { get; set; }
        public int Cost { get; set; }
        public int UnitPrice { get; set; }
        public int Price { get; set; }
        // 区分
        public string Dmy1 { get; set; }
    }
}
