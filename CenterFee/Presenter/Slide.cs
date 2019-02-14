using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CenterFee.Presenter
{
    public partial class Slide : Form
    {
        protected Form Referer;
        protected Form NextForm;

        override protected void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if (null != Referer)
            {
                Referer.Close();
            }
        }

        public void Back()
        {
            this.Hide();
            Referer.Show();
        }

        public void Next()
        {
            Hide();
            NextForm.Show();
        }
    }
}
