using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Shipment
{
    class Schema : AbstractSchema
    {
        private static readonly CsvHelper.Configuration.Configuration CsvConf = new CsvHelper.Configuration.Configuration()
        {
            HasHeaderRecord = false
        };
        private static readonly Dictionary<string, string> StoreNameByCode = FetchStoreCodeAndName();
        private static readonly Func<Source, DataRow, Result> ResultSelector = (a, b) =>
        {
            return new Result()
            {
                DeliveredAt = a.DeliveredAt,
                NewItemAge = String.Empty,
                SupplierCode = b["supplier_code"].ToString(),
                SupplierName = b["supplier_name"].ToString(),
                VarietyCode = b["item_code"].ToString(),
                ModelNo = b["model_no"].ToString(),
                ProductName = b["product_name"].ToString(),
                JanCode = a.JanCode,
                ExpectedQty = a.ExpectedQty,
                ActualQty = a.ActualQty,
                Price = int.Parse(b["price"].ToString()),
                StoreCode = a.DeliveryDestinationCode,
                StoreName = StoreNameByCode[a.DeliveryDestinationCode],
                ReservedItem = a.IsReservedItem ? "客注" : String.Empty,
                PurchaseOrderNumber = a.PurchaseOrderNumber,
            };
        };

        public List<Source> SourceRecords { get; private set; }
        public override dynamic DenormalizedSchema => DenormalizedSchema_;

        private List<Result> DenormalizedSchema_;

        public Schema(string path)
        {
            Path = path;
            Name = "事前出荷";
        }

        public override void Denormalize()
        {
            GetSourceRecords();
            DataTable products = FetchProducts(SourceRecords.Select(r => r.JanCode).Distinct());

            var cmp = EqualityComparer<string>.Default;
            var alookup = SourceRecords.ToLookup((x) => x.JanCode, cmp);
            var blookup = products.AsEnumerable().ToLookup((x) => x["jan"].ToString(), cmp);

            var keys = new HashSet<string>(alookup.Select(p => p.Key), cmp);
            var query = from key in keys
                        from xa in alookup[key].DefaultIfEmpty(null)
                        from xb in blookup[key].DefaultIfEmpty(null)
                        select ResultSelector(xa, xb);
            DenormalizedSchema_ = query.ToList();
        }

        public override void Write(TextWriter writer)
        {
            using (var csv = new CsvHelper.CsvWriter(writer))
            {
                // ヘッダーあり
                csv.Configuration.HasHeaderRecord = true;
                // マッパーを登録
                csv.Configuration.RegisterClassMap<Mapping>();
                // データを読み出し
                csv.WriteRecords(DenormalizedSchema_);
            }
        }

        private void GetSourceRecords()
        {
            if (SourceRecords != null) return;

            using (var streamReader = new StreamReader(Path))
            using (var csv = new CsvHelper.CsvReader(streamReader, CsvConf))
            {
                //csv.Configuration.RegisterClassMap<SourceMapper>();
                csv.Configuration.ReadingExceptionOccurred = ex =>
                {
                    // Do something instead of throwing an exception.
                    return true;
                };
                SourceRecords = csv.GetRecords<Source>().ToList();
            }
        }

        private static Dictionary<string, string> FetchStoreCodeAndName()
        {
            const string tableName = "stores";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    var sql = @"SELECT `stores`.`code` AS `store_code`,
                                       `stores`.`name` AS `store_name`
                                  FROM `humpty_dumpty`.`stores`;";
                    var adapter = new MySqlDataAdapter(sql, conn);
                    var data = new DataSet();
                    adapter.Fill(data, tableName);
                    var stores = data.Tables[tableName];
                    return stores.AsEnumerable()
                        .ToDictionary(row => row["store_code"].ToString(),
                                      row => row["store_name"].ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return null;
        }

        //private sealed class SourceMapper : CsvHelper.Configuration.ClassMap<Source>
        //{
        //    public SourceMapper()
        //    {
        //        Map(m => m.ExpectedQty).Index(1).TypeConverterOption.NumberStyles(NumberStyles.Integer);
        //        Map(m => m.DeliveryDestinationCode).Index(2);
        //        Map(m => m.JanCode).Index(4);
        //        Map(m => m.ActualQty).Index(5).TypeConverterOption.NumberStyles(NumberStyles.Integer);
        //        Map(m => m.DeliveredAt).Index(6).TypeConverterOption.Format("yyyy-MM-dd");
        //    }
        //}
    }
}
