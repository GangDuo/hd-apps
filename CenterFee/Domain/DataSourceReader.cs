using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using T = Text;

namespace CenterFee.Domain
{
    internal class DataSourceReader
    {
        public event Action<DataTable> LoadAsDataTableCompleted;

        private string XlsPath;
        private string SheetName;

        public DataSourceReader(string xlsPath, string sheetName)
        {
            XlsPath = xlsPath ?? throw new ArgumentNullException(nameof(xlsPath));
            SheetName = sheetName ?? throw new ArgumentNullException(nameof(sheetName));
        }

        /**
         * エクセルファイルの指定シートからDataTableとしてデータ読み出し
         *
         */
        public void LoadAsDataTableAsync()
        {
            TaskScheduler taskScheduler;
            try
            {
                taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            }
            catch (Exception)
            {
                taskScheduler = TaskScheduler.Current;
            }

            Task.Factory.StartNew(() =>
            {
                return LoadAsDataTable();
            }).ContinueWith((task) =>
            {
                var tbl = task.Result;
                if (null != LoadAsDataTableCompleted)
                {
                    LoadAsDataTableCompleted(tbl);
                }
                return tbl;
            }, taskScheduler);
        }

        // 入力エクセルを読み込める形に作り変える
        private static void Sanitize(string xls, string sheetName)
        {
            if (!Util.Sanitize(xls, sheetName))
            {
                throw new Exception();
            }
        }

        private DataTable LoadAsDataTable()
        {
            var fileName = T.RandomString.Generate(new T.RandomString.Options() { Length = 16 });
            var copyXlsFile = Path.Combine(Path.GetTempPath(), fileName);
            try
            {
                File.Copy(XlsPath, copyXlsFile);
                Sanitize(copyXlsFile, SheetName);
                var reader = new ExcelDataSourceReader() { FullName = copyXlsFile, SheetName = SheetName };
                return reader.Execute();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                File.Delete(copyXlsFile);
            }
            return new DataTable();
        }

        private class ExcelDataSourceReader : MsOffice.AbstractRunnableSql
        {
            public string FullName { get; set; }
            public string SheetName { get; set; }
            //public string CommandText { get; set; }

            public DataTable Execute()
            {
                return Execute(FullName, null);
            }

            private static void DestroyIfSupplierCodeFieldIsNotNumeric(DataTable table)
            {
                // code列が数値でないレコードは削除
                foreach (DataRow row in table.Rows)
                {
                    if (!Domain.Util.IsNumeric(row[Entity.Literal.SupplierCodeField].ToString()))
                    {
                        row.Delete();
                    }
                }
                table.AcceptChanges();
            }

            protected override DataTable RunSql(System.Data.Common.DbCommand command, object arg)
            {
                command.CommandText = String.Format(@"
                                        SELECT *
                                          FROM [{0}$]
                                         WHERE (LEN([{1}]) > 0) AND (([{2}] > 0) OR ([{3}] > 0) OR ([{4}] > 0))",
                                          SheetName,
                                          Entity.Literal.SupplierCodeField,
                                          Entity.Literal.FeeField,
                                          Entity.Literal.CobaltCenterFeeAmountField,
                                          Entity.Literal.EdiField);
                try
                {
                    DataTable table = new DataTable();
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                    DestroyIfSupplierCodeFieldIsNotNumeric(table);
                    StampPaymentDateField(table);
                    return table;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                throw new Exception();
            }

            // 支払日情報を追加
            private static void StampPaymentDateField(DataTable table)
            {
                var atypicalCodes = new int[] { 749 };
                var suppliers = new Supplier();
                suppliers.Load();
                foreach (DataRow row in table.Rows)
                {
                    var rawCode = int.Parse(row[Entity.Literal.SupplierCodeField].ToString());
                    // 取引先コードは原則4桁であるが、5桁の取引先コードが数社あるため読み替えする。
                    var code = String.Format(atypicalCodes.Contains(rawCode) ? "{0:0000}0" : "{0:0000}", rawCode);
                    var supplier = suppliers.FindByCode(code);
                    var value = supplier["payment_date"].ToString() + "払い";
                    row[Entity.Literal.PaymentDateField] = value;
                }
                table.AcceptChanges();
            }
        }
    }
}
