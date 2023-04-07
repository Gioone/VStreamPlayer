using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VStreamPlayer.MVVM.Models
{
    public class VideoInfo
    {
        /// <summary>
        /// Video thump
        /// </summary>
        public Bitmap Thump { get; set; }

        /// <summary>
        /// Video title
        /// </summary>
        public string Titile { get; set; }

        /// <summary>
        /// Video last time
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Video size
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Video format
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Video path
        /// </summary>
        public string Path { get; set; }
    }
}
