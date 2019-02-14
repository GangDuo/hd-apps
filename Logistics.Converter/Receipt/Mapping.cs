using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Receipt
{
    sealed class Mapping : ClassMap<Result>
    {
        public Mapping()
        {
            Map(x => x.Dmy1).Index(0).Name("区分");
            Map(x => x.StoreCode).Index(1).Name("店舗コード");
            Map(x => x.StoreName).Index(2).Name("店舗名");
            Map(x => x.DeliveredAt).Index(3).Name("納品日").TypeConverterOption.Format("yyyy-MM-dd");
            Map(x => x.SupplyChainManagementCode).Index(4).Name("SCM番号");
            Map(x => x.VarietyCode).Index(5).Name("部門");
            Map(x => x.SupplierCode).Index(6).Name("仕入先");
            Map(x => x.JanCode).Index(7).Name("JAN");
            Map(x => x.ModelNo).Index(8).Name("品番");
            Map(x => x.ProductName).Index(9).Name("品名");
            Map(x => x.PurchasedAt).Index(10).Name("発注日");
            Map(x => x.Qty).Index(11).Name("出荷数");
            Map(x => x.UnitCost).Index(12).Name("原単価");
            Map(x => x.Cost).Index(13).Name("原価金額");
            Map(x => x.UnitPrice).Index(14).Name("売単価");
            Map(x => x.Price).Index(15).Name("売価金額");
        }
    }
}
