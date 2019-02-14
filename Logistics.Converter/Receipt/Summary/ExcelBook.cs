using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter.Receipt.Summary
{
    class ExcelBook : AbstractExcelBook
    {
        private static ClassMap Mapping = new Mapping();

        protected override string TemplateFullName =>  String.Empty;
        protected override ClassMap ClassMap => Mapping;
        protected override AbstractSchema Schema => null;
        protected override string SheetName => "SCM合計";

        private AbstractSchema SchemaInstance;

        public ExcelBook(AbstractSchema schema)
        {
            SchemaInstance = schema;
        }

        public override void SaveAs(string path)
        {
            var schema = SchemaInstance as Schema;
            new ExcelAsOleDb(path, SheetName)
            {
                InsertSql = InsertSql,
                InsertCommandParameters = InsertCommandParameters
            }
            .RegisterTableEditor(TableEditor, (object)schema.DenormalizedSummarySchema)
            .Save();
        }
    }
}
