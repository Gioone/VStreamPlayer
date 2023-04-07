using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VStreamPlayer.Core
{
    /// <summary>
    /// This class is used for segment play.
    /// </summary>
    internal class SegmentPlay
    {
        public TimeSpan StartTime{ get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
