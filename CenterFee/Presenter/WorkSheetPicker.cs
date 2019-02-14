using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CenterFee.Presenter
{
    public partial class WorkSheetPicker : Form
    {
        public string PickedName { get; set; }
        public string XlsPath { get; set; }

        public WorkSheetPicker()
        {
            InitializeComponent();
        }

        private void WorkSheetPicker_Load(object sender, EventArgs e)
        {
            picLoading.Visible = true;
            btnOk.Enabled = false;
            var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {
                return Domain.Util.GetExcelSheetNames(XlsPath);
            }).ContinueWith((task) =>
            {
                listBox1.DataSource = task.Result;
                picLoading.Visible = false;
                btnOk.Enabled = true;
            }, taskScheduler);

            listBox1.DataBindings.Add(new Binding(
                                     "SelectedItem",
                                     this,
                                     "PickedName",
                                     true));
        }
    }
}
