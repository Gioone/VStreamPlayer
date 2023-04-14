using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VStreamPlayer.MVVM.Models
{
    public partial class PlayListItemModel : ObservableObject
    {
        static PlayListItemModel()
        {
            DefaultBackground = VStreamPlayer.Helper.Resource.FindColorResource("SecondaryColorBrush");
            PlayingBackground = VStreamPlayer.Helper.Resource.FindColorResource("MainColorBrush");
        }

        /// <summary>
        /// Background
        /// </summary>
        [ObservableProperty]
        public Brush _background = DefaultBackground;

        /// <summary>
        /// Item default background
        /// </summary>
        public static Brush DefaultBackground { get; set; }

        /// <summary>
        /// Background of when video is playing
        /// </summary>
        public static Brush PlayingBackground { get; set; }

        /// <summary>
        /// Video path.
        /// </summary>
        public string VideoPath
        {
            get; set;
        }

        /// <summary>
        /// An order to display.
        /// </summary>
        [ObservableProperty] 
        public int _order;

        /// <summary>
        /// Video thumb image
        /// </summary>
        public ImageSource Thumb
        {
            get; set;
        }


        /// <summary>
        /// Video title.
        /// </summary>
        public string Title
        {
            get; set;
        }

        /// <summary>
        /// Video duration
        /// </summary>
        public string Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Video size
        /// </summary>
        public string Size
        {
            get;
            set;
        }

        /// <summary>
        /// Video formats
        /// </summary>
        public string Format
        {
            get; set;
        }


        /// <summary>
        /// Initialized a PlayListItemView
        /// </summary>
        /// <param name="path">File path</param>
        /// <exception cref="FileNotFoundException">File Not Found</exception>
        /// <exception cref="FileFormatException">Invalid file format</exception>
        public PlayListItemModel(string path)
        {

            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            VideoPath = path;

            Task<bool> task = Task.Run(() => VStreamPlayer.Core.HelperUtils.CheckIsValidVideo(path));
            // Task<Task<bool>> task = Task.Factory.StartNew(() => (VStreamPlayer.Core.HelperUtils.CheckIsValidVideo(path)));
            task.Wait();
            if (!task.Result)
            {
                throw new FileFormatException();
            }

            Task<string> formats = Task.Run(() => FFMpeg.FFMpegHelper.GetVideoFormat(path));
            formats.Wait();
            Format = formats.Result.ToLower();

            Task<string> taskVideoLastClock = Task.Run(() => FFMpeg.FFMpegHelper.GetVideoDurationClock(path));

            taskVideoLastClock.Wait();

            Duration = taskVideoLastClock.Result;

            BitmapSource bs = VStreamPlayer.Helper.Media.GetThumbnailByPath(path);

            Thumb = bs;

            // Thumb = new SWC.Image();
            // Thumb.Source = bs;

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
