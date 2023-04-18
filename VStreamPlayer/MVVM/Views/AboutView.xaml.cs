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
            // 这里可以做一个 BUG，当窗口关闭后主界面再单击后会报异常。
            // Close();
            Hide();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
