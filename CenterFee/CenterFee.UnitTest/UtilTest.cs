using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CenterFee.UnitTest
{
    [TestClass]
    public class UtilTest
    {
        [TestMethod]
        public void OpenExcel()
        {
            var ps = Process.GetProcessesByName("excel");
            CenterFee.Domain.Util.OpenExcel(System.IO.Path.GetFullPath(@"data\empty.xlsx"));
            Assert.IsTrue((Process.GetProcessesByName("excel").Length - ps.Length) == 1);

            //配列から1つずつ取り出す
            foreach (Process p in Process.GetProcessesByName("excel"))
            {
                //IDとメインウィンドウのキャプションを出力する
                Console.WriteLine("{0}/{1}", p.Id, p.MainWindowTitle);
                if (System.Text.RegularExpressions.Regex.IsMatch(p.MainWindowTitle, @"empty\.xlsx"))
                {
                    p.CloseMainWindow();
                }
            }
        }

        [TestMethod]
        public void GetExcelSheetNames()
        {
            var expected = Enumerable.Range(1, 3).Select(n => "Sheet" + n).ToArray();
            var actual = CenterFee.Domain.Util.GetExcelSheetNames(System.IO.Path.GetFullPath(@"data\empty.xlsx"));
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsNumeric()
        {
            Assert.IsTrue(CenterFee.Domain.Util.IsNumeric("001"));
            Assert.IsFalse(CenterFee.Domain.Util.IsNumeric("1a"));
        }
    }
}
