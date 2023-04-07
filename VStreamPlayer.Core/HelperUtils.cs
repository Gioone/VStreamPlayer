using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VStreamPlayer.FFMpeg;
using Size = System.Windows.Size;

namespace VStreamPlayer.Core
{
    /// <summary>
    /// Some tools related to the UI
    /// </summary>
    public static class HelperUtils
    {
        /// <summary>
        /// Convert a UserControl to an icon and save it to specified path.
        /// </summary>
        /// <param name="uc"><see cref="UserControl"/></param>
        /// <param name="savePath">Saved path.</param>
        /// <param name="iconWidth">Icon width.</param>
        /// <param name="iconHeight">Icon height.</param>
        public static void SaveUserControlToIcon(UserControl uc, string savePath, int iconWidth, int iconHeight)
        {
            uc.Measure(new Size(iconWidth, iconHeight));
            uc.Arrange(new Rect(new Size(iconWidth, iconHeight)));
            var bitmap = new RenderTargetBitmap((int)uc.ActualWidth, (int)uc.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(uc);
            var frame = BitmapFrame.Create(bitmap);

            using var stream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(frame);
            encoder.Save(stream);

            System.Drawing.Image image = new Bitmap(stream);

            using (MemoryStream msImg = new MemoryStream()
                   , msIco = new MemoryStream())
            {
                image.Save(msImg, ImageFormat.Png);

                using (var bin = new BinaryWriter(msIco))
                {
                    //写图标头部
                    bin.Write((short)0);      //0-1保留
                    bin.Write((short)1);      //2-3文件类型。1=图标, 2=光标
                    bin.Write((short)1);      //4-5图像数量（图标可以包含多个图像）

                    bin.Write((byte)image.Width); //6图标宽度
                    bin.Write((byte)image.Height); //7图标高度
                    bin.Write((byte)0);      //8颜色数（若像素位深>=8，填0。这是显然的，达到8bpp的颜色数最少是256，byte不够表示）
                    bin.Write((byte)0);      //9保留。必须为0
                    bin.Write((short)0);      //10-11调色板
                    bin.Write((short)32);     //12-13位深
                    bin.Write((int)msImg.Length); //14-17位图数据大小
                    bin.Write(22);         //18-21位图数据起始字节

                    //写图像数据
                    bin.Write(msImg.ToArray());

                    bin.Flush();
                    bin.Seek(0, SeekOrigin.Begin);
                    Icon icon = new Icon(msIco);

                    using var fs = new FileStream(savePath, FileMode.Create);

                    icon.Save(fs);
                }
            }
        }

        /// <summary>
        /// Get the parent control based on the specified <see cref="DependencyObject" />
        /// </summary>
        /// <typeparam name="T">FrameworkElement</typeparam>
        /// <param name="obj">Specified <see cref="DependencyObject" /></param>
        /// <returns>Searched the parent control</returns>
        public static T GetParentControl<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T element)
                {
                    return element;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        public static T GetChildControl<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject child;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T t)
                {
                    return t;
                }
                else
                {
                    GetChildControl<T>(child);
                }
                
            }
            return null;
        }

        /// <summary>
        /// Open a single file, and return the file path.
        /// </summary>
        /// <returns>The file path.</returns>
        public static string OpenSingleVideo()
        {
            // string title = new FrameworkElement().TryFindResource("OpenFile") as string ?? "Open file";
            string title = VStreamPlayer.Helper.Resource.FindStringResource("OpenFile", "Open file");
            OpenFileDialog ofd = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                DereferenceLinks = true,
                CheckPathExists = true,
                Multiselect = false,
                Title = title,
            };

            ofd.ShowDialog();

            return ofd.FileName;
        }

        /// <summary>
        /// Open multi files, and return the file paths.
        /// </summary>
        /// <returns>File paths</returns>
        public static string[] OpenMultiVideos()
        {
            // string title = new FrameworkElement().TryFindResource("OpenFile") as string ?? "Open file";
            string title = VStreamPlayer.Helper.Resource.FindStringResource("OpenFile", "Open file");
            OpenFileDialog ofd = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                DereferenceLinks = true,
                CheckPathExists = true,
                Multiselect = true,
                Title = title,
            };

            ofd.ShowDialog();

            return ofd.FileNames;
        }

        /// <summary>
        /// Current video from path is valid video or not. <see langword="true" /> is a valid video. <see langword="false" /> is not a valid video.
        /// </summary>
        /// <param name="videoPath"></param>
        /// <returns><see langword="true" /> is a valid video. <see langword="false" /> is not a valid video.</returns>
        public static async Task<bool> CheckIsValidVideo(string videoPath)
        {
            string videoFormats = await FFMpegHelper.GetVideoFormat(videoPath);

            if (string.IsNullOrEmpty(videoFormats))
            {
                return false;
            }

            videoFormats = videoFormats.ToLower();

            string[] strFormats = videoFormats.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var format in strFormats)
            {

                bool b = VideoEncodingType.VideoEncodingList.Any(x => x.Equals(format, StringComparison.OrdinalIgnoreCase));
                if (b)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
