using System.Windows;
using VStreamPlayer.Window.MessageBox;

namespace VStreamPlayer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            string strInfo = VStreamPlayer.Helper.Resource.FindStringResource("ErrorString", "Error");

            string strConfirm = VStreamPlayer.Helper.Resource.FindStringResource("ConfirmString", "Confirm");


            new MessageBoxInfo(strInfo, e.Exception.Message, strConfirm).ShowDialog();
            Application.Current.Shutdown();
        }
    }
}
