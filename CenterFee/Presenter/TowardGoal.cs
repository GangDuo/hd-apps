using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CenterFee.Presenter
{
    public partial class TowardGoal : Form
    {
        public string DestinationFolder { get; set; }

        private Form Referer;
        public DataTable DataSource;
        private Domain.DataWriter Writer;

        public TowardGoal(Form referer)
        {
            InitializeComponent();
            Referer = referer;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Debug.Assert(DataSource.AsEnumerable().All(row => Domain.Util.IsNumeric(row[Entity.Literal.SupplierCodeField].ToString())));

            progressBar1.Maximum = DataSource.Rows.Count;

            Writer = new Domain.DataWriter();
            Writer.WriteAsync(DataSource, DestinationFolder);
            Writer.ProgressChange += () =>
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    progressBar1.PerformStep();
                });
            };
            Writer.Completed += (args) =>
            {
                EnableControl();
                this.Close();
                if (args.Cancelled)
                {
                    MessageBox.Show("キャンセルされました");
                }
                else if (null != args.Error)
                {
                    MessageBox.Show(args.Error.Message);
                }
                else
                {
                    MessageBox.Show("終了");
                }
            };
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Writer.Cancel();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Referer.Show();
        }

        private void Toggle(bool enable)
        {
           // this.btnBack.Enabled = enable;
           // this.btnRun.Enabled = enable;
        }

        private void EnableControl()
        {
            Toggle(true);
        }

        private void DisableControl()
        {
            Toggle(false);
        }
    }
}
