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

namespace VStreamPlayer.Assets.UserControls
{
    /// <summary>
    /// AboutUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class AboutUserControl : UserControl
    {
        public AboutUserControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            if (sender is Hyperlink link)
            {
                string strLink = link.NavigateUri.ToString();
                try
                {
                    Process.Start(strLink);
                }
                catch
                {

                }
            }
        }
    }
}
