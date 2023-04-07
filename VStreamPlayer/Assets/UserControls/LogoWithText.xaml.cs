using System.Windows;

namespace VStreamPlayer.Assets.UserControls
{
    /// <summary>
    /// LogoWithText.xaml 的交互逻辑
    /// </summary>
    public partial class LogoWithText : System.Windows.Controls.UserControl
    {
        public double TextSize
        {
            get { return (double)GetValue(TextSizeProperty); }
            set { SetValue(TextSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextSizeProperty =
            DependencyProperty.Register("TextSize", typeof(double), typeof(LogoWithText), new PropertyMetadata(12.0));


        public LogoWithText()
        {
            InitializeComponent();
        }
    }
}
