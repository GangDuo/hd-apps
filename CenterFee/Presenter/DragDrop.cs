using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CenterFee.Presenter
{
    internal class DragDrop
    {
        public string[] AllowExt { get; set; }
        public bool MultiFile { get; set; }

        public void ApplyEffects(DragEventArgs e)
        {
            string[] fileNameArray = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (!e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {
                /* ファイル以外のドラッグは受け入れない */
                e.Effect = DragDropEffects.None;
                return;
            }

            if (!MultiFile && fileNameArray.Length > 1)
            {
                /* 複数ファイルのドラッグは受け入れない */
                e.Effect = DragDropEffects.None;
                return;
            }
            foreach (var item in fileNameArray)
            {
                if (!AllowExt.Select(x => x.ToLower()).Contains(Path.GetExtension(item).ToLower()))
                {
                    /* 拡張子が.xls以外は受け入れない */
                    e.Effect = DragDropEffects.None;
                    return;
                }
            }

            /* 上記以外は受け入れる */
            e.Effect = DragDropEffects.All;
        }
    }
}
