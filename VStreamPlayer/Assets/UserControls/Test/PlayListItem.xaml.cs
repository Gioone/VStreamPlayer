using System.Drawing;
using System.IO;
using System.Windows;
using SWC = System.Windows.Controls;
using System.Windows.Media.Imaging;
using System;
using VStreamPlayer.Helper;
using System.Threading.Tasks;

namespace VStreamPlayer.Assets.UserControls.Test
{
    /// <summary>
    /// PlayListItem.xaml 的交互逻辑
    /// </summary>
    public partial class PlayListItem : SWC.UserControl
    {

        /// <summary>
        /// An order to display.
        /// </summary>
        public int Order
        {
            get { return (int) GetValue(OrderProperty); }

            set { SetValue(OrderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Order.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrderProperty =
            DependencyProperty.Register(nameof(Order), typeof(int), typeof(PlayListItem));

        /// <summary>
        /// Video thumb image
        /// </summary>
        public SWC.Image Thumb
        {
            get { return (SWC.Image)GetValue(ThumbProperty); }
            set { SetValue(ThumbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thumb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbProperty =
            DependencyProperty.Register(nameof(Thumb), typeof(SWC.Image), typeof(PlayListItem));


        /// <summary>
        /// Video title.
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(PlayListItem));


        /// <summary>
        /// Video duration
        /// </summary>
        public string Duration
        {
            get { return (string)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(string), typeof(PlayListItem));


        /// <summary>
        /// Video size
        /// </summary>
        public string Size
        {
            get { return (string)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(string), typeof(PlayListItem));


        /// <summary>
        /// Video formats
        /// </summary>
        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Format.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register(nameof(Format), typeof(string), typeof(PlayListItem));

        /// <summary>
        /// Initialized a PlayListItem
        /// </summary>
        /// <param name="path">File path</param>
        /// <exception cref="FileNotFoundException">File Not Found</exception>
        /// <exception cref="FileFormatException">Invalid file format</exception>
        public PlayListItem(string path)
        {
            InitializeComponent();

            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            Task<bool> task = Task.Run(() => VStreamPlayer.Core.HelperUtils.CheckIsValidVideo(path));
            task.Wait();

            if (!task.Result)
            {
                throw new FileFormatException();
            }

            Task<string> formats = Task.Run(() => FFMpeg.FFMpegHelper.GetVideoFormat(path));
            formats.Wait();

            // string formats = FFMpeg.FFMpegHelper.GetVideoFormat(path);
            Format = formats.Result.ToLower();

            Task<string> taskVideoLastClock = Task.Run(() =>  FFMpeg.FFMpegHelper.GetVideoDurationClock(path) );

            taskVideoLastClock.Wait();

            Duration = taskVideoLastClock.Result;

            BitmapSource bs = VStreamPlayer.Helper.Media.GetThumbnailByPath(path);

            Thumb = new SWC.Image();
            Thumb.Source = bs;

            string strTitle = Path.GetFileNameWithoutExtension(path);
            Title = strTitle;

            decimal dFileLength = GetFileLength(path);
            Size = dFileLength + "";
        }

        /// <summary>
        /// Get file length (MB).
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>File length (MB).</returns>
        public decimal GetFileLength(string path)
        {
            FileInfo info = new FileInfo(path);
            long lFileSize = info.Length;
            decimal dFileSize = (decimal)lFileSize;
            dFileSize = dFileSize / 1024 / 1024;
            dFileSize = Math.Round(dFileSize, 2, MidpointRounding.AwayFromZero);
            // string strFileSize = dFileSize + "";
            // Size = strFileSize;

            return dFileSize;
        }
    }
}
