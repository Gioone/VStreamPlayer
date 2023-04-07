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

namespace VStreamPlayer.Assets.UserControls
{
    /// <summary>
    /// Delete.xaml 的交互逻辑
    /// </summary>
    public partial class Delete : UserControl
    {
        public double WidthAndHeight
        {
            get { return (double)GetValue(WidthAndHeightProperty); }
            set { SetValue(WidthAndHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WidthAndHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthAndHeightProperty =
            DependencyProperty.Register(nameof(WidthAndHeight), typeof(double), typeof(Delete), new PropertyMetadata(20.0));

        public SolidColorBrush Color
        {
            get { return (SolidColorBrush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color),
                typeof(SolidColorBrush),
                typeof(Delete),
                new PropertyMetadata(new SolidColorBrush(Colors.White)));
        public Delete()
        {
            InitializeComponent();
        }
    }
}
