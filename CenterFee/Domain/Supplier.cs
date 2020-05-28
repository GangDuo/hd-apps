using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace CenterFee.Domain
{
    internal class Supplier
    {
        private static readonly string ConnectionString = new MySqlConnectionStringBuilder()
        {
            Server = ConfigurationManager.AppSettings["db:server"],
            Port = uint.Parse(ConfigurationManager.AppSettings["db:port"]),
            UserID = ConfigurationManager.AppSettings["db:user"],
            Password = ConfigurationManager.AppSettings["db:password"],
            Database = ConfigurationManager.AppSettings["db:database"],
            ConvertZeroDateTime = true,
        }.ToString();

        private static readonly string TableName = "suppliers";
        private DataSet Raw = new DataSet();

        public void Load()
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    // Perform database operations
                    string sql = String.Format("SELECT * FROM `{0}`;", TableName);
                    var adapter = new MySqlDataAdapter(sql, conn);
                    MySqlCommandBuilder cb = new MySqlCommandBuilder(adapter);

                    //var data = new DataSet();
                    adapter.Fill(Raw, TableName);
                    Console.WriteLine(Raw.Tables[TableName].Rows.Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

        }

        public DataRow FindByCode(string code)
        {
            return Raw.Tables[TableName].AsEnumerable()
                .Where(row => row["code"].ToString() == code)
                .FirstOrDefault();
        }
    }
}
