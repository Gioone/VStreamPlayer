using System.Windows;
using System.Windows.Controls;
namespace VStreamPlayer.Window.MessageBox
{
    /// <summary>
    /// MessageBoxOkAndCancel.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBoxOkAndCancel : System.Windows.Window
    {
        /// <summary>
        /// Window return result. <see langword="true" /> user clicked positive button. <see langword="false" /> user clicked negative button.
        /// </summary>
        public bool ReturnResult { get; set; }

        public MessageBoxOkAndCancel(string title, string text, string positiveButtonText, string negativeButtonText)
        {
            Title = title;
            Text = text;
            PositiveButtonText = positiveButtonText;
            NegativeButtonText = negativeButtonText;
        }

        /// <summary>
        /// Negative button text.
        /// </summary>
        public string NegativeButtonText
        {
            get { return (string)GetValue(NegativeButtonTextProperty); }
            set { SetValue(NegativeButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NegativeButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NegativeButtonTextProperty =
            DependencyProperty.Register(nameof(NegativeButtonText), typeof(string), typeof(MessageBoxOkAndCancel), new PropertyMetadata(""));

        /// <summary>
        /// Positive button text.
        /// </summary>
        public string PositiveButtonText
        {
            get { return (string)GetValue(PositiveButtonTextProperty); }
            set { SetValue(PositiveButtonTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PositiveButtonText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PositiveButtonTextProperty =
            DependencyProperty.Register(nameof(PositiveButtonText), typeof(string), typeof(MessageBoxOkAndCancel), new PropertyMetadata(""));

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
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(MessageBoxOkAndCancel), new PropertyMetadata(""));


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
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MessageBoxOkAndCancel), new PropertyMetadata(""));

        public MessageBoxOkAndCancel()
        {
            InitializeComponent();
        }



        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
