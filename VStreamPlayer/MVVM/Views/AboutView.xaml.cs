using System.Windows;
using System.Windows.Input;

namespace VStreamPlayer.MVVM.Views
{
    /// <summary>
    /// AboutUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class AboutView : System.Windows.Window
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
