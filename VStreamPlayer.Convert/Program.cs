using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VStreamPlayer.Convert
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            /*
            Logo logo = new Logo();
            FrameworkElement element = logo.Content as FrameworkElement;
            RenderTargetBitmap bitmap = new RenderTargetBitmap(256, 256, 96d, 96d, PixelFormats.Default);

            if (element is null)
            {
                Console.WriteLine("Convert False!");
                return;
            }

            bitmap.Render(element);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var fs = File.Open(@"D:\logo.png", FileMode.Create))
            {
                encoder.Save(fs);
            }
            */
            /*
            Image img = new Image();

            Logo logo = new Logo();

            img.Source = logo;
            */
        }
    }
}
