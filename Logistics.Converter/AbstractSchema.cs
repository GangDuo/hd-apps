using CsvHelper.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text.Extensions;

namespace Logistics.Converter
{
    abstract class AbstractSchema
    {
        protected static readonly string ConnectionString = new MySqlConnectionStringBuilder()
        {
            Server = ConfigurationManager.AppSettings["db:server"],
            Port = uint.Parse(ConfigurationManager.AppSettings["db:port"]),
            UserID = ConfigurationManager.AppSettings["db:user"],
            Password = ConfigurationManager.AppSettings["db:password"],
            Database = ConfigurationManager.AppSettings["db:database"],
            ConvertZeroDateTime = true,
        }.ToString();

        public abstract dynamic DenormalizedSchema { get; }
        public string Name { get; protected set; }
        //public bool IsNormalizedAtOutput { get; set; } = true;
        protected string Path;

        protected static DataTable FetchProducts(IEnumerable<string> codes)
        {
            const string tableName = "products";
            using (var conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    var sql = @"SELECT `products`.`jan`,
                                       `products`.`name` AS `product_name`,
                                       `products`.`itemCD` AS `item_code`,
                                       `products`.`model_number` AS `model_no`,
                                       `products`.`price`,
                                       `suppliers`.`code` AS `supplier_code`,
                                       `suppliers`.`name` AS `supplier_name`
                                  FROM `humpty_dumpty`.`products`
                             LEFT JOIN `humpty_dumpty`.`suppliers` ON `products`.`vendorCD` = `suppliers`.`code`
                                 WHERE `products`.`jan` IN(" + String.Join(",", codes.Select(r => r.SingleQuote())) + ");";
                    var adapter = new MySqlDataAdapter(sql, conn);
                    var data = new DataSet();
                    adapter.Fill(data, tableName);
                    return data.Tables[tableName];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return null;
        }

        public abstract void Denormalize();
        public abstract void Write(TextWriter writer);
        public virtual void Write(string path)
        {
            using (var sw = new StreamWriter(path, true, Encoding.GetEncoding("SHIFT_JIS")))
            {
                Write(sw);
            }
        }
    }
}
