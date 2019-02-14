using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using T = Text;

namespace CenterFee.Presenter
{
    public partial class DataSourceViewer : Slide//, INotifyPropertyChanged
    {
        private Color dgvBgColor;
        private CommonOpenFileDialog FolderBrowserDialog;
        //private string _FileName;
        //private string FileName
        //{
        //    get { return _FileName; }
        //    set
        //    {
        //    if (value != _FileName)
        //    {
        //        // ※必ずfirstに値を設定してからNotifyPropertyChangeを呼び出すこと
        //        _FileName = value;
        //
        //        // NotifyPropertyChangeの引数に設定するのは"プロパティ名"
        //        NotifyPropertyChanged("FileName");
        //    }
        //}
        //}

        public DataSourceViewer(Form referer)
        {
            InitializeComponent();
            
            FolderBrowserDialog = new CommonOpenFileDialog();
            FolderBrowserDialog.Title = "作業フォルダの選択";
            FolderBrowserDialog.IsFolderPicker = true;
            FolderBrowserDialog.InitialDirectory = Entity.Literal.DestinationFolder;//this.textBox1.Text;
            FolderBrowserDialog.AddToMostRecentlyUsedList = false;
            FolderBrowserDialog.AllowNonFileSystemItems = false;
            FolderBrowserDialog.DefaultDirectory = this.textBox1.Text;
            FolderBrowserDialog.EnsureFileExists = true;
            FolderBrowserDialog.EnsurePathExists = true;
            FolderBrowserDialog.EnsureReadOnly = false;
            FolderBrowserDialog.EnsureValidNames = true;
            FolderBrowserDialog.Multiselect = false;
            FolderBrowserDialog.ShowPlacesList = true;

            textBox1.Text = Entity.Literal.DestinationFolder;

            Referer = referer;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvBgColor = this.dataGridView1.BackgroundColor;
        }

        private void dataGridView1_DragLeave(object sender, EventArgs e)
        {
            this.dataGridView1.BackgroundColor = dgvBgColor;
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Orange;

            new DragDrop()
            {
                AllowExt = new string[] { ".xls", ".xlsx" },
                MultiFile = false
            }.ApplyEffects(e);
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            this.dataGridView1.BackgroundColor = dgvBgColor;

            string[] fileNameArray = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            var path = fileNameArray[0];
            var picker = new WorkSheetPicker() { XlsPath = path };
            if (DialogResult.Cancel == picker.ShowDialog())
            {
                return;
            }

            var source = new Domain.DataSourceReader(path, picker.PickedName);
            source.LoadAsDataTableAsync();
            source.LoadAsDataTableCompleted += (tbl) =>
            {
                dataGridView1.DataSource = tbl;
                btnRun.Enabled = true;
            };
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            this.Hide();
            new TowardGoal(this)
                {
                    DataSource= (DataTable)dataGridView1.DataSource,
                    DestinationFolder = textBox1.Text
                }.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // TODO : データバインディングしたい
            if (FolderBrowserDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.textBox1.Text = FolderBrowserDialog.FileName;
                Debug.WriteLine(FolderBrowserDialog.FileName);
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void NotifyPropertyChanged(String info)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(info));
        //}
    }
}
