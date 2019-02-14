using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Receipt
{
    class Schema : AbstractSchema
    {
        public override dynamic DenormalizedSchema => DenormalizedSchema_;
        public IEnumerable<Summary.Result> DenormalizedSummarySchema
        {
            get
            {
                return DenormalizedSchema_?.GroupBy(x => new
                {
                    x.StoreCode,
                    x.StoreName,
                    x.DeliveredAt,
                    x.SupplyChainManagementCode
                }).Select(x =>
                {
                    return new Summary.Result()
                    {
                        StoreCode = x.Key.StoreCode,
                        StoreName = x.Key.StoreName,
                        DeliveredAt = x.Key.DeliveredAt,
                        SupplyChainManagementCode = x.Key.SupplyChainManagementCode,
                        TotalQty = x.Sum(a => a.Qty),
                        TotalCost = x.Sum(a => a.Cost),
                        TotalPrice = x.Sum(a => a.Price)
                    };
                });
            }
        }

        private List<Result> DenormalizedSchema_;

        public Schema(string path)
        {
            Path = path;
            Name = "納品書明細";
        }

        public override void Denormalize()
        {
            var conf = new CsvHelper.Configuration.Configuration()
            {
                HasHeaderRecord = false
            };
            using (var streamReader = new StreamReader(Path, Encoding.GetEncoding("SHIFT_JIS")))
            using (var csv = new CsvHelper.CsvReader(streamReader, conf))
            {
                csv.Configuration.RegisterClassMap<SourceMapper>();
                csv.Configuration.ReadingExceptionOccurred = ex =>
                {
                    // Do something instead of throwing an exception.
                    return true;
                };
                var source = csv.GetRecords<Source>().ToList();
                DataTable products = FetchProducts(source.Select(r => r.JanCode).Distinct());

                Func<Source, DataRow, Result> resultSelector = (a, b) =>
                {
                    return new Result()
                    {
                        DeliveredAt = a.DeliveredAt,
                        SupplyChainManagementCode = a.SupplyChainManagementCode,
                        SupplierCode = b["supplier_code"].ToString(),
                        VarietyCode = a.VarietyCode,
                        ModelNo = b["model_no"].ToString(),
                        ProductName = b["product_name"].ToString(),
                        JanCode = a.JanCode,
                        StoreCode = a.StoreCode,
                        StoreName = a.StoreName,
                        Qty = a.Qty,
                        UnitCost = a.UnitCost,
                        Cost = a.Cost,
                        UnitPrice = a.UnitPrice,
                        Price = a.Price,
                    };
                };
                var cmp = EqualityComparer<string>.Default;
                var alookup = source.ToLookup((x) => x.JanCode, cmp);
                var blookup = products.AsEnumerable().ToLookup((x) => x["jan"].ToString(), cmp);

                var keys = new HashSet<string>(alookup.Select(p => p.Key), cmp);
                var query = from key in keys
                            from xa in alookup[key].DefaultIfEmpty(null)
                            from xb in blookup[key].DefaultIfEmpty(null)
                            select resultSelector(xa, xb);
                DenormalizedSchema_ = query.ToList();
            }
        }

        public override void Write(string path)
        {
            using (var writer = new StreamWriter(path, true, Encoding.GetEncoding("SHIFT_JIS")))
            using (var csv = new CsvHelper.CsvWriter(writer))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<Mapping>();
                csv.WriteRecords(DenormalizedSchema_);
            }

            string file = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), Program.Prefix + "SCM合計" + System.IO.Path.GetExtension(path));
            using (var writer = new StreamWriter(file, true, Encoding.GetEncoding("SHIFT_JIS")))
            using (var csv = new CsvHelper.CsvWriter(writer))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<Summary.Mapping>();
                csv.WriteRecords(DenormalizedSummarySchema);
            }
        }

        public override void Write(TextWriter writer)
        {
            throw new NotImplementedException();
        }

        private sealed class SourceMapper : CsvHelper.Configuration.ClassMap<Source>
        {
            public SourceMapper()
            {
                Map(m => m.StoreCode).Index(1);
                Map(m => m.StoreName).Index(2);
                Map(m => m.DeliveredAt).Index(5).TypeConverterOption.Format("yyyyMMdd");
                Map(m => m.JanCode).Index(10);
                Map(m => m.SupplyChainManagementCode).Index(12);
                Map(m => m.Qty).Index(13).TypeConverterOption.NumberStyles(NumberStyles.AllowDecimalPoint);
                Map(m => m.UnitCost).Index(14).TypeConverterOption.NumberStyles(NumberStyles.AllowDecimalPoint);
                Map(m => m.Cost).Index(15).TypeConverterOption.NumberStyles(NumberStyles.AllowDecimalPoint);
                Map(m => m.UnitPrice).Index(16).TypeConverterOption.NumberStyles(NumberStyles.AllowDecimalPoint);
                Map(m => m.Price).Index(17).TypeConverterOption.NumberStyles(NumberStyles.AllowDecimalPoint);
                Map(m => m.VarietyCode).Index(19);
            }
        }
    }
}
