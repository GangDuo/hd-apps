using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter
{
    abstract class AbstractExcelBook
    {
        abstract protected string TemplateFullName { get; }
        abstract protected ClassMap ClassMap { get; }
        abstract protected AbstractSchema Schema { get; }

        protected List<string> ColumnNameAll => InsertCommandParameters.Select(x => x.SourceColumn).ToList();
        protected string InsertSql => String.Format(@"INSERT INTO [{0}$] ({1}) VALUES ({2})", SheetName, String.Join(", ", ColumnNameAll), String.Join(", ", Enumerable.Repeat("?", ColumnNameAll.Count)));
        protected dynamic DenormalizedSchema => Schema?.DenormalizedSchema;
        protected　virtual string SheetName => Schema?.Name;
        protected List<OleDbParameter> InsertCommandParameters
        {
            get
            {
                // InsertCommandParameters_を汎化
                var parameters = new List<OleDbParameter>();
                var maps = ClassMap.MemberMaps.OrderBy(x => x.Data.Index);
                //var forObject = item;
                foreach (var m in maps)
                {
                    foreach (var (fieldName, i) in m.Data.Names.Select((v, i) => (v, i)))
                    {
                        System.Diagnostics.Debug.WriteLine(fieldName);
                        Type t = null;
                        var memberInfo = m.Data.Member;
                        switch (memberInfo.MemberType)
                        {
                            case System.Reflection.MemberTypes.Field:
                                var fi = (System.Reflection.FieldInfo)memberInfo;
                                t = fi.FieldType;
                                break;
                            case System.Reflection.MemberTypes.Property:
                                var pi = (System.Reflection.PropertyInfo)memberInfo;
                                t = pi.PropertyType;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        //var paramName = String.Format("@{0:00}", i + 1);
                        var paramName = String.Format("@{0}", memberInfo.Name);
                        var p = new OleDbParameter(paramName, OleDbTypeMap.GetType(t), 0, fieldName);
                        parameters.Add(p);
                    }
                }
                return parameters;
            }
        }

        public virtual void SaveAs(string path)
        {
            File.Copy(TemplateFullName, path);

            new ExcelAsOleDb(path, SheetName)
            {
                InsertSql = InsertSql,
                InsertCommandParameters = InsertCommandParameters
            }
            .RegisterTableEditor(TableEditor, (object)DenormalizedSchema)
            .Save();
        }

        protected void TableEditor(DataTable table, object arg)
        {
            foreach (var item in arg as IEnumerable<object>)
            {
                DataRow newRow = table.NewRow();
                //var maps = ClassMap.MemberMaps.OrderBy(x => x.Data.Index);
                var maps = ClassMap.MemberMaps;
                var forObject = item;
                foreach (var i in maps)
                {
                    foreach (var fieldName in i.Data.Names)
                    {
                        dynamic value = null;
                        var memberInfo = i.Data.Member;
                        switch (memberInfo.MemberType)
                        {
                            case System.Reflection.MemberTypes.Field:
                                var fi = (System.Reflection.FieldInfo)memberInfo;
                                value = fi.GetValue(forObject);
                                break;
                            case System.Reflection.MemberTypes.Property:
                                var pi = (System.Reflection.PropertyInfo)memberInfo;
                                value = pi.GetValue(forObject);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        var value2 = value;
                        if (i.Data.TypeConverterOptions.Formats?.Length > 0)
                        {
                            value2 = value.ToString(i.Data.TypeConverterOptions.Formats.First());
                        }
                        newRow[fieldName] = value2;
                    }
                }
                table.Rows.Add(newRow);
            }
        }

        private static class OleDbTypeMap
        {
            private static readonly Dictionary<Type, OleDbType> TypeMap = new Dictionary<Type, OleDbType> {
                {typeof(string), OleDbType.VarChar },
                {typeof(long), OleDbType.BigInt },
                {typeof(byte[]), OleDbType.Binary },
                {typeof(bool), OleDbType.Boolean },
                {typeof(decimal), OleDbType.Decimal },
                {typeof(DateTime), OleDbType.Date },
                {typeof(TimeSpan), OleDbType.DBTime },
                {typeof(double), OleDbType.Double },
                {typeof(Exception),OleDbType.Error },
                {typeof(Guid), OleDbType.Guid },
                {typeof(int), OleDbType.Integer },
                {typeof(float), OleDbType.Single },
                {typeof(short), OleDbType.SmallInt },
                {typeof(sbyte), OleDbType.TinyInt },
                {typeof(ulong), OleDbType.UnsignedBigInt },
                {typeof(uint), OleDbType.UnsignedInt },
                {typeof(ushort), OleDbType.UnsignedSmallInt },
                {typeof(byte), OleDbType.UnsignedTinyInt }
            };
            public static OleDbType GetType(Type type) => TypeMap[type];
        }
    }
}
