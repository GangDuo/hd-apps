using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Receipt.Summary
{
    sealed class Mapping : CsvHelper.Configuration.ClassMap<Result>
    {
        public Mapping()
        {
            Map(x => x.Dmy1).Index(0).Name("区分");
            Map(x => x.StoreCode).Index(1).Name("店舗コード");
            Map(x => x.StoreName).Index(2).Name("店舗名");
            Map(x => x.DeliveredAt).Index(3).Name("納品日").TypeConverterOption.Format("yyyy-MM-dd");
            Map(x => x.SupplyChainManagementCode).Index(4).Name("SCM番号");
            Map(x => x.TotalQty).Index(5).Name("出荷数");
            Map(x => x.TotalCost).Index(6).Name("原価金額");
            Map(x => x.TotalPrice).Index(7).Name("売価金額");
        }
    }
}
