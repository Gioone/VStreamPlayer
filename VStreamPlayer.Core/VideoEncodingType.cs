using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VStreamPlayer.Core
{
    public struct VideoEncodingType
    {
        public static readonly string[] VideoEncodingList = 
            { "mp4", "mov", "3gp", "wmv", "flv", "ogg", "webm", "avi", "m4v", "mkv" };

        public static readonly string MP4 = "mp4";
        public static readonly string MOV = "mov";
        public static readonly string _3GP = "3gp";
        public static readonly string WMV = "wmv";
        public static readonly string FLV = "flv";

        public static readonly string OGG = "ogg";
        public static readonly string WEBM = "webm";
        public static readonly string AVI = "avi";
        public static readonly string M4V = "m4v";
        public static readonly string MKV = "mkv";

    }
}
