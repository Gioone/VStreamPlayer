using System;
using System.Collections.Generic;
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

namespace VStreamPlayer.Assets.UserControls.Test
{
    /// <summary>
    /// TestTabItem.xaml 的交互逻辑
    /// </summary>
    public partial class TestTabItem : UserControl
    {

        public bool IsCheckedProperty
        {
            get { return (bool)GetValue(IsCheckedPropertyProperty); }
            set { SetValue(IsCheckedPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCheckedProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedPropertyProperty =
            DependencyProperty.Register("IsCheckedProperty", typeof(bool), typeof(TestTabItem));




        public TestTabItem()
        {
            InitializeComponent();
        }
    }
}
