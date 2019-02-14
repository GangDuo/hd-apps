using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace Logistics.Converter.Stock
{
    class ExcelBook : AbstractExcelBook
    {
        private static ClassMap Mapping = new Mapping();

        protected override string TemplateFullName => System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "在庫表.xlsx");
        protected override ClassMap ClassMap => Mapping;
        protected override AbstractSchema Schema => SchemaInstance;
        protected override string SheetName => "センター在庫表";

        private AbstractSchema SchemaInstance;

        public ExcelBook(AbstractSchema schema)
        {
            SchemaInstance = schema;
        }
    }
}
