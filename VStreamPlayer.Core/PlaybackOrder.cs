using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VStreamPlayer.Core
{
    /// <summary>
    /// Indicated an order that video playback
    /// </summary>
    public enum PlaybackOrder
    {
        /// <summary>
        /// Sequential play
        /// </summary>
        SequentialPlay,

        /// <summary>
        /// Single loop play
        /// </summary>
        SingleLoop,

        /// <summary>
        /// Random play
        /// </summary>
        RandomPlay
    }
}
