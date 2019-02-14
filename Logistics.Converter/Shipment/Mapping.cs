using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Shipment
{
    sealed class Mapping : CsvHelper.Configuration.ClassMap<Result>
    {
        public Mapping()
        {
            Map(x => x.DeliveredAt).Index(0).Name("出荷日").TypeConverterOption.Format("yyyy-MM-dd");
            Map(x => x.NewItemAge).Index(1).Name("投入表番号");
            Map(x => x.StoreCode).Index(2).Name("店舗コード");
            Map(x => x.StoreName).Index(3).Name("店舗名");
            Map(x => x.SupplierCode).Index(4).Name("仕入先CD");
            Map(x => x.SupplierName).Index(5).Name("仕入先名");
            Map(x => x.VarietyCode).Index(6).Name("部門");
            Map(x => x.ModelNo).Index(7).Name("品番");
            Map(x => x.ProductName).Index(8).Name("品名");
            Map(x => x.JanCode).Index(9).Name("JANコード");
            Map(x => x.ExpectedQty).Index(10).Name("予定数");
            Map(x => x.ActualQty).Index(11).Name("実績数");
            Map(x => x.Price).Index(12).Name("売単価");
            Map(x => x.ReservedItem).Index(13).Name("客注");
            Map(x => x.PurchaseOrderNumber).Index(14).Name("発注番号");
        }
    }
}
