using System.Windows;
using System.Windows.Media;

namespace VStreamPlayer.Helper
{
    public class Resource
    {
        /// <summary>
        /// Find a resource string based on the specified key.
        /// </summary>
        /// <param name="key">Specified key</param>
        /// <param name="dft">A value that if can not find.</param>
        /// <returns>A resource string</returns>
        public static string FindStringResource(string key, string dft = "")
        {
            return new FrameworkElement().TryFindResource(key) as string ?? dft;
        }

        /// <summary>
        /// Find color resource.
        /// </summary>
        /// <param name="key">Specified key.</param>
        /// <returns>A brush</returns>
        public static Brush FindColorResource(string key)
        {
            if (new FrameworkElement().TryFindResource(key) is Brush brush)
            {
                return brush;
            }

            return new SolidColorBrush(Colors.Transparent);
        }
    }
}
