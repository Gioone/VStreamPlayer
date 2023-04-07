using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Shell;

namespace VStreamPlayer.Helper
{
    public class Media
    {
        /// <summary>
        /// Get a file thumbnail.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>A <see cref="BitmapSource" /></returns>
        public static BitmapSource GetThumbnailByPath(string filePath)
        {
            ShellFile shellFile = ShellFile.FromFilePath(filePath);
            // Bitmap thumbnail = shellFile.Thumbnail.ExtraLargeBitmap;
            BitmapSource bs = shellFile.Thumbnail.MediumBitmapSource;
            return bs;
        }
    }
}
