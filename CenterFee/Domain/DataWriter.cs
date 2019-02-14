using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using T = Text;

namespace CenterFee.Domain
{
    internal class DataWriter
    {
        public event Action<AsyncCompletedEventArgs> Completed;
        public event Action ProgressChange;

        private CancellationTokenSource TokenSource2;

        protected virtual void OnCompleted(AsyncCompletedEventArgs args)
        {
            if (null != Completed)
            {
                Completed(args);
            }
        }

        protected virtual void OnProgressChange()
        {
            if (null != ProgressChange)
            {
                ProgressChange();
            }
        }

        public void Cancel()
        {
            if (null != TokenSource2)
            {
                TokenSource2.Cancel();
            }
        }

        public void WriteAsync(DataTable tbl, string destinationFolder)
        {
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            TokenSource2 = new CancellationTokenSource();
            CancellationToken ct = TokenSource2.Token;
            Task.Factory.StartNew(() =>
            {
                Exception e = null;
                try
                {
                    foreach (DataRow record in tbl.Rows)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            ct.ThrowIfCancellationRequested();
                        }

                        Domain.DataWriter.WriteData(record, destinationFolder);
                        OnProgressChange();
                    }
                }
                catch (Exception ex)
                {
                    e = ex;
                }
                return new AsyncCompletedEventArgs(e, ct.IsCancellationRequested, null);
            }, ct).ContinueWith((task) =>
            {
                OnCompleted(task.Result);
                TokenSource2.Dispose();
                TokenSource2 = null;
            }, taskScheduler); ;
        }

        private static void WriteData(DataRow record, string destinationFolder)
        {
            var fileName = T.RandomString.Generate(new T.RandomString.Options() { Length = 16 }) + ".xlsx";
            var xlsFile = Path.Combine(Path.GetTempPath(), fileName);
            var catalog = new Catalog(record);
            File.Copy(catalog.Template, xlsFile);
            MsOffice.MsExcel.SetValue(xlsFile, catalog.Mapping.Create(record));

            // 所定の場所へ移動
            Debug.Assert(null != destinationFolder);
            Directory.CreateDirectory(Path.Combine(destinationFolder, catalog.SubFolder));
            var dst = Path.Combine(destinationFolder, catalog.SubFolder, String.Format("{0:0000} {1} 御中.xlsx", catalog.SupplierCode, catalog.SupplierName));
            File.Move(xlsFile, dst);
            //MsOffice.MsExcel.SaveAsPdf(dst);
            //File.Delete(dst);
        }

        private class Catalog
        {
            public string Template { get; private set; }
            public IExcelMapping Mapping { get; private set; }
            public int SupplierCode { get; private set; }
            public string SupplierName { get; private set; }
            public string SubFolder { get; private set; }

            public Catalog(DataRow record)
            {
                var tbl = record.Table;
                SubFolder = record[Entity.Literal.PaymentDateField].ToString();
                SupplierCode = int.Parse(record[Entity.Literal.SupplierCodeField].ToString());
                SupplierName = record[Entity.Literal.SupplierNameField].ToString();
                Template = Entity.Literal.XlsTmpl;
                Mapping = new MappingForTamamura();
            }
        }
    }
}
