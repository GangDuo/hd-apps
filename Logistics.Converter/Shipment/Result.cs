using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Shipment
{
    class Result
    {
        //[JsonProperty("deliveredAt", Required = Required.Always)]
        //[Index(0)]
        //[Name("納品日")]
        //[DateTimeStyles(System.Globalization.DateTimeStyles.None)]
        //[Format("yyyy-MM-dd")]
        public DateTime DeliveredAt { get; set; }
        //[JsonProperty("newItemAge", Required = Required.Default)]
        //[Index(1)]
        //[Name("投入表番号")]
        public string NewItemAge { get; set; }
        //[JsonProperty("storeCode", Required = Required.Always)]
        //[Index(2)]
        //[Name("店舗コード")]
        public string StoreCode { get; set; }
        //[JsonProperty("storeName", Required = Required.Always)]
        //[Index(3)]
        //[Name("店舗名")]
        public string StoreName { get; set; }
        //[JsonProperty("supplierCode", Required = Required.Always)]
        //[Index(4)]
        //[Name("仕入先CD")]
        public string SupplierCode { get; set; }
        //[JsonProperty("supplierName", Required = Required.Always)]
        //[Index(5)]
        //[Name("仕入先名")]
        public string SupplierName { get; set; }
        //[JsonProperty("varietyCode", Required = Required.Always)]
        //[Index(6)]
        //[Name("部門")]
        public string VarietyCode { get; set; }
        //[JsonProperty("modelNo", Required = Required.Always)]
        //[Index(7)]
        //[Name("品番")]
        public string ModelNo { get; set; }
        //[JsonProperty("productName", Required = Required.Always)]
        //[Index(8)]
        //[Name("品名")]
        public string ProductName { get; set; }
        //[JsonProperty("janCode", Required = Required.Always)]
        //[Index(9)]
        //[Name("JANコード")]
        public string JanCode { get; set; }
        //[JsonProperty("expectedQty", Required = Required.Always)]
        //[Index(10)]
        //[Name("予定数")]
        public int ExpectedQty { get; set; }
        //[JsonProperty("actualQty", Required = Required.Always)]
        //[Index(11)]
        //[Name("実績数")]
        public int ActualQty { get; set; }
        //[JsonProperty("price", Required = Required.Always)]
        //[Index(12)]
        //[Name("売単価")]
        public int Price { get; set; }
        public string ReservedItem { get; set; }
        public string PurchaseOrderNumber { get; set; }
    }
}
