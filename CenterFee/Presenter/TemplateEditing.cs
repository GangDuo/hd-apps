using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CenterFee.Presenter
{
    public partial class TemplateEditing : Slide
    {
        public TemplateEditing()
        {
            InitializeComponent();
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            Domain.Util.OpenExcel(Entity.Literal.XlsTmpl);
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            Domain.Util.OpenExcel(Entity.Literal.XlsTmplCobalt);
        }

        private void linkLabel3_Click(object sender, EventArgs e)
        {
            Domain.Util.OpenExcel(Entity.Literal.XlsTmplDeposit);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void TemplateEditing_Load(object sender, EventArgs e)
        {
            NextForm = new DataSourceViewer(this);
        }
    }
}
