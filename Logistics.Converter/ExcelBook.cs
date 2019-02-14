using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Converter
{
    class ExcelBook
    {
        private static readonly string Cmd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "Excel.wsf");
        private string FullName;

        public ExcelBook(string path)
        {
            FullName = path;
        }

        public void GenerateFromJson(string json)
        {
            // Excel出力（新規プロセス）
            var fi = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string startup_path = fi.Directory.FullName;
            var xlsPath = Path.Combine(startup_path, "_tmpl.xlsx");
            File.Copy(Path.Combine(startup_path, "Scripts", "tmpl.xlsx"), xlsPath, true);

            var Generator = new ProcessStartInfo()
            {
                FileName = @"cscript",
                Arguments = String.Format(@"//B //Nologo ""{0}"" --file ""{1}""", Cmd, xlsPath),
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            using (var hProcess = Process.Start(Generator))
            {
                hProcess.StandardInput.Write(json);
                hProcess.StandardInput.Flush();
                hProcess.StandardInput.Close();

                hProcess.WaitForExit();
                var StdOut = hProcess.StandardOutput.ReadToEnd();
                var StdErr = hProcess.StandardError.ReadToEnd();
            }

        }
    }
}
