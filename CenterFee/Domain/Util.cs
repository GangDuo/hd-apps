using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CenterFee.Domain
{
    internal class Util
    {
        private static readonly string Cmd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "EXCEL.JS");

        public static void OpenExcel(string xlsPath)
        {
            using (var hProcess = Process.Start(new ProcessStartInfo()
            {
                FileName = @"cscript",
                Arguments = String.Format(@"//B //Nologo ""{0}"" ""{1}""", Cmd, xlsPath),
                WindowStyle = ProcessWindowStyle.Hidden
            }))
            {
                hProcess.WaitForExit();
            }
        }

        public static List<string> GetExcelSheetNames(string xlsPath)
        {
            List<string> sheetNames = null;
            var cmd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "printExcelSheetName.js");
            using (var hProcess = Process.Start(new ProcessStartInfo()
            {
                FileName = @"cscript",
                Arguments = String.Format(@"//B //Nologo ""{0}"" ""{1}""", cmd, xlsPath),
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                //出力を読み取れるようにする
                UseShellExecute = false,
                RedirectStandardOutput = true
            }))
            {
                var result = hProcess.StandardOutput.ReadToEnd();
                hProcess.WaitForExit();
                sheetNames = new List<string>(result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            }
            return sheetNames;
        }

        public static bool IsNumeric(string stTarget)
        {
            double dNullable;

            return double.TryParse(
                stTarget,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }

        public static bool Sanitize(string xlsPath, string sheetName)
        {
            bool isSuccess = false;
            var cmd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "sanitize-excel.js");
            using (var hProcess = Process.Start(new ProcessStartInfo()
            {
                FileName = @"cscript",
                Arguments = String.Format(@"//B //Nologo ""{0}"" ""{1}"" ""{2}""", cmd, xlsPath, sheetName),
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                //出力を読み取れるようにする
                UseShellExecute = false,
                RedirectStandardOutput = true
            }))
            {
                var stdOut = hProcess.StandardOutput.ReadToEnd();
                hProcess.WaitForExit();
                isSuccess = stdOut.Trim().ToLower() == "true";
            }
            return isSuccess;
        }
    }
}
