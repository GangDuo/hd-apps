using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Stock
{
    class Mapping : CsvHelper.Configuration.ClassMap<Result>
    {
        public Mapping()
        {
            Map(x => x.JanCode).Index(0).Name("JAN");
            Map(x => x.SupplierCode).Index(1).Name("仕入先");
            Map(x => x.SupplierName).Index(2).Name("仕入先名");
            Map(x => x.VarietyCode).Index(3).Name("部門");
            Map(x => x.ModelNo).Index(4).Name("商品");
            Map(x => x.ProductName).Index(5).Name("品名");
            Map(x => x.Price).Index(6).Name("売単価");
            Map(x => x.Qty).Index(7).Name("出荷可能数");
            Map(x => x.Dmy2).Index(8).Name("入荷予定数");
            Map(x => x.Dmy1).Index(9).Name("次回入荷予定日");
//            Map(x => x.Dmy3).Index(10).Name("不良在庫");
        }
    }
}
