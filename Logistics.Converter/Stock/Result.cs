using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Stock
{
    class Result
    {
        // 仕入先
        //[JsonProperty("supplierCode", Required = Required.Always)]
        public string SupplierCode { get; set; }
        // 仕入先名
        //[JsonProperty("supplierName", Required = Required.Always)]
        public string SupplierName { get; set; }
        // 部門
        //[JsonProperty("varietyCode", Required = Required.Always)]
        public string VarietyCode { get; set; }
        // 商品
        //[JsonProperty("modelNo", Required = Required.Always)]
        public string ModelNo { get; set; }
        // JAN
        //[JsonProperty("jan", Required = Required.Always)]
        public string JanCode { get; set; }
        // 品名
        //[JsonProperty("productName", Required = Required.Always)]
        public string ProductName { get; set; }
        // 売単価
        //[JsonProperty("price", Required = Required.Always)]
        public int Price { get; set; }
        // 出荷可能数
        //[JsonProperty("qty", Required = Required.Always)]
        public int Qty { get; set; }

        public string Dmy1 { get; set; }
        public string Dmy2 { get; set; }
        public string Dmy3 { get; set; }
    }
}
