using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter
{
    class Program
    {
        public static string Prefix
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd_HHmmss_");
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0) { return; }
            foreach (var file in args)
            {
                if (!File.Exists(file)) { continue; }

                try
                {
                    var schema = SchemaFactory.Create(file);
                    schema.Denormalize();
                    var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Prefix + schema.Name + ".xlsx");
                    var xlsx = ExcelBookFactory.CreateFrom(schema);
                    xlsx.SaveAs(filename);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}: {1}", file, ex.Message);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
            // 終了
            //Console.ReadKey();
        }
    }
}
