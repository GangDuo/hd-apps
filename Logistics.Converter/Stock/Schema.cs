using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Stock
{
    class Schema : AbstractSchema
    {
        public override dynamic DenormalizedSchema => DenormalizedSchema_;

        private List<Result> DenormalizedSchema_;

        public Schema(string path)
        {
            Path = path;
            Name = "在庫表";
        }

        public override void Denormalize()
        {
            using (var streamReader = new StreamReader(Path))
            using (var csv = new CsvHelper.CsvReader(streamReader))
            {
                // CSVに対して事前処理を行う。（JAN毎に在庫数量を合計）
                var source = csv.GetRecords<Source>()
                    .GroupBy(r=> r.JanCode)
                    .Select(g => new Source() { JanCode = g.Key, Qty = g.Sum(r => r.Qty) })
                    .ToList();
                DataTable products = FetchProducts(source.Select(r => r.JanCode).Distinct());

                Func<Source, DataRow, Result> resultSelector = (a, b) =>
                {
                    return new Result()
                    {
                        JanCode = a.JanCode,
                        Qty = a.Qty,
                        SupplierCode = b["supplier_code"].ToString(),
                        SupplierName = b["supplier_name"].ToString(),
                        VarietyCode = b["item_code"].ToString(),
                        ModelNo = b["model_no"].ToString(),
                        ProductName = b["product_name"].ToString(),
                        Price = int.Parse(b["price"].ToString()),
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

        public override void Write(TextWriter writer)
        {
            using (var csv = new CsvHelper.CsvWriter(writer))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<Mapping>();
                csv.WriteRecords(DenormalizedSchema_);
            }
        }
    }
}
