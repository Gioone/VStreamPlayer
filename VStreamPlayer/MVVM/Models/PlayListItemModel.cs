using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        [JsonIgnore]
        public Brush _background = DefaultBackground;

        /// <summary>
        /// Item default background
        /// </summary>
        [JsonIgnore]
        public static Brush DefaultBackground { get; set; }

        /// <summary>
        /// Background of when video is playing
        /// </summary>
        [JsonIgnore]
        public static Brush PlayingBackground { get; set; }

        /// <summary>
        /// Add time
        /// </summary>
        public DateTime AddTime
        { get; set; }

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
        [JsonIgnore]
        public int _order;

        /// <summary>
        /// Video thumb image
        /// </summary>
        [JsonIgnore]
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
        public decimal Size
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
                throw new FileNotFoundException();
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

            string strTitle = System.IO.Path.GetFileNameWithoutExtension(path);
            Title = strTitle;

            decimal dFileLength = GetFileLength(path);
            Size = dFileLength;

            AddTime = DateTime.Now;
        }

        public PlayListItemModel(DateTime addTime, string videoPath, int order, string title, string duration, decimal size, string format)
        {
            Task<bool> taskIsExisted = Task.Run(() => Core.HelperUtils.CheckIsValidVideo(videoPath));
            taskIsExisted.Wait();
            if (!taskIsExisted.Result)
            {
                throw new FileNotFoundException();
            }

            AddTime = addTime;
            VideoPath = videoPath;
            Order = order;
            Title = title;
            Duration = duration;
            Size = size;
            Format = format;

            BitmapSource bs = VStreamPlayer.Helper.Media.GetThumbnailByPath(videoPath);

            Thumb = bs;
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
