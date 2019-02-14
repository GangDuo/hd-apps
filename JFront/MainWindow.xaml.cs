using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JFront
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModels.MainWindow GlobalState;

        public MainWindow()
        {
            InitializeComponent();

            calendar1.SelectedDate = DateTime.Today;
            calendar1.DisplayDate = DateTime.Today;

            this.GlobalState = new ViewModels.MainWindow() { CategoryType = Models.FilterType.None };
            this.DataContext = GlobalState;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var isEmpty = new string[]
            {
                Properties.Settings.Default.SIGN_IN_URL,
                Properties.Settings.Default.ORGANIZATION_CODE,
                Properties.Settings.Default.USER_CODE,
                Properties.Settings.Default.ORGANIZATION_PASS,
                Properties.Settings.Default.USER_PASS,
            }.Any(x => String.IsNullOrWhiteSpace(x));
            if (isEmpty)
            {
                ShowDialogOfSettings();
                return;
            }

            var args = String.Format("/year:{0} /month:{1} /day:{2} /base-url:{3} /organization-code:{4} /organization-pass:{5} /user-code:{6} /user-pass:{7}",
                                     this.calendar1.SelectedDate.Value.Year,
                                     this.calendar1.SelectedDate.Value.Month,
                                     this.calendar1.SelectedDate.Value.Day,
                                     Properties.Settings.Default.SIGN_IN_URL,
                                     Properties.Settings.Default.ORGANIZATION_CODE,
                                     Properties.Settings.Default.ORGANIZATION_PASS,
                                     Properties.Settings.Default.USER_CODE,
                                     Properties.Settings.Default.USER_PASS);
            if (!GlobalState.CategoryType.Equals(Models.FilterType.None))
            {
                args += String.Format("/line:{0}", GlobalState.CategoryType.ToString().Substring(4, 2));
            }
            using (var hProcess = Process.Start(new ProcessStartInfo()
            {
                FileName = @"wscript",
                Arguments = String.Format(@"//B //Nologo main.jse {0}", args),
            }))
            {
                //hProcess.WaitForExit();
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            ShowDialogOfSettings();
        }

        private void ShowDialogOfSettings()
        {
            new Views.Settings()
            {
                Owner = this
            }.ShowDialog();
        }
    }
}
