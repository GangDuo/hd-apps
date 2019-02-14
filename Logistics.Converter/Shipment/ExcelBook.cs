using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace Logistics.Converter.Shipment
{
    class ExcelBook : AbstractExcelBook
    {
        private static ClassMap Mapping = new Mapping();

        protected override string TemplateFullName => System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "事前出荷.xlsx");
        protected override ClassMap ClassMap => Mapping;
        protected override AbstractSchema Schema => SchemaInstance;

        private AbstractSchema SchemaInstance;

        public ExcelBook(AbstractSchema schema)
        {
            SchemaInstance = schema;
        }
    }
}
