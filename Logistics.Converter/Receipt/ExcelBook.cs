using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Receipt
{
    class ExcelBook : AbstractExcelBook
    {
        private static ClassMap Mapping = new Mapping();

        protected override string TemplateFullName => System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "納品書明細.xlsx");
        protected override ClassMap ClassMap => Mapping;
        protected override AbstractSchema Schema => SchemaInstance;
        protected override string SheetName => "明細";

        private AbstractSchema SchemaInstance;

        public ExcelBook(AbstractSchema schema)
        {
            SchemaInstance = schema;
        }

        public override void SaveAs(string path)
        {
            base.SaveAs(path);

            var ins = new Summary.ExcelBook(SchemaInstance);
            ins.SaveAs(path);
        }
    }
}
