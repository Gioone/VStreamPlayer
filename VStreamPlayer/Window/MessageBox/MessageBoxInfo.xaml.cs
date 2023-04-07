using System.Windows;
using System.Windows.Controls;
namespace VStreamPlayer.Window.MessageBox
{
    /// <summary>
    /// MessageBoxOkAndCancel.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxInfo : System.Windows.Window
    {
        public MessageBoxInfo(string title, string text, string confirmButtonText)
        {
            InitializeComponent();
            Title = title;
            Text = text;
            ConfirmButtonText = confirmButtonText;
        }


        /// <summary>
        /// Positive button text.
        /// </summary>
        public string ConfirmButtonText
        {
            get { return (string)GetValue(ConfirmButtonTextProperty); }
            set { SetValue(ConfirmButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfirmButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfirmButtonTextProperty =
            DependencyProperty.Register(nameof(ConfirmButtonText), typeof(string), typeof(MessageBoxInfo), new PropertyMetadata(""));

        /// <summary>
        /// Window title
        /// </summary>
        public new string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(MessageBoxInfo), new PropertyMetadata(""));


        /// <summary>
        /// Text content.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MessageBoxInfo), new PropertyMetadata(""));

        public MessageBoxInfo()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
