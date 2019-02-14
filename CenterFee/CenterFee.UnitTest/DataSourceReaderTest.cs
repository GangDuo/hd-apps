using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Threading.Tasks;

namespace CenterFee.UnitTest
{
    [TestClass]
    public class DataSourceReaderTest
    {
        [TestMethod]
        public void LoadAsDataTable()
        {
            bool completed = false;
            DataTable source = null;
            var reader = new CenterFee.Domain.DataSourceReader(@"data\DataSource.xlsx", "Sheet1");
            Action<DataTable> onLoadAsDataTableCompleted = null;
            onLoadAsDataTableCompleted = (table) =>
            {
                reader.LoadAsDataTableCompleted -= onLoadAsDataTableCompleted;
                source = table;
                completed = true;
            };
            reader.LoadAsDataTableCompleted += onLoadAsDataTableCompleted;
            reader.LoadAsDataTableAsync();

            var task = Task.Factory.StartNew(() =>
            {
                while (!completed)
                {
                    System.Threading.Thread.Sleep(1000);
                }
                return source;
            }, TaskCreationOptions.AttachedToParent);
            task.Wait();
            Assert.IsTrue(task.Result.Rows.Count == 5);
        }
    }
}
