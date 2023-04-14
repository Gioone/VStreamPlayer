using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using FFMpegCore;

namespace VStreamPlayer.FFMpeg
{
    public class FFMpegHelper
    {
        private static readonly object _lock = new object();

        private static string FFMpegPath
        {
            get
            {
                if (string.IsNullOrEmpty(_ffMpegPath))
                {
                    lock (_lock)
                    {
                        if (string.IsNullOrEmpty(_ffMpegPath))
                        {
                            _ffMpegPath = Path.Combine(System.Environment.CurrentDirectory, "ffmpeg");
                        }
                    }
                }

                return _ffMpegPath;
            }
        }

        private static string _ffMpegPath;

        static FFMpegHelper()
        {
            // string ffmpegPath = @"D:\C#\wpf\Application\VStreamPlayer\VStreamPlayer\bin\Debug\ffmpeg";


            // FFMpegPath = ffmpegPath;
        }

        /*
        public static void SetFFMpegPath(string path)
        {
            if (string.IsNullOrEmpty(FFmpeg.ExecutablesPath))
            {
                FFmpeg.SetExecutablesPath(path);
            }
        }
        */

        public static async void GetSnapshot()
        {
            
            // var bitmap = FFMpegCore.FFMpeg.Snapshot("inputPath", new Size(2,4), TimeSpan.FromMinutes(1));
        }


        /// <summary>
        /// Get video duration clock like "14:15:27"
        /// </summary>
        /// <param name="path">Video path</param>
        /// <returns>Video duration clock string.</returns>
        public static string GetVideoDurationClock(string path)
        {
            if (!File.Exists(FFMpegPath + "/ffprobe.exe"))
            {
                throw new FileNotFoundException("This application relies on FFMpeg, but we can't find it in the application's folder. The application will exit.");
            }

            Task<StringBuilder> task = Task.Run(async () =>
            {
                using var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = FFMpegPath + "/ffprobe.exe",
                        Arguments = $"-i \"{path}\" -show_entries format=duration -v quiet -of csv=\"p=0\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                proc.Start();

                StringBuilder sb = new StringBuilder();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = await proc.StandardOutput.ReadLineAsync();
                    sb.AppendLine(line);
                }

                return sb;
            });
            task.Wait();

            StringBuilder sb = task.Result;

            // Like 127.1234. 127 is second, and 1234 is millisecond.
            string strDurationClock = sb.ToString();
            string[] arrDuration = strDurationClock.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrDuration.Length == 1)
            {
                long.TryParse(strDurationClock, out long ticks);
                return VStreamPlayer.Helper.Convert.MillisecondsToClock(ticks);
            }
            else if (arrDuration.Length == 2)
            {
                string strFormat = $"{strDurationClock:0.000}";
                // string[] arrDuration = strDurationClock.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                double.TryParse(strFormat, out double dMilliseconds);
                dMilliseconds *= 1000;
                long lDuration = Convert.ToInt64(dMilliseconds);
                return VStreamPlayer.Helper.Convert.MillisecondsToClock(lDuration);

            }

            return sb.ToString();
        }

        public static async Task<string> GetVideoFormat(string path)
        {
            // string strCmd = $"-v quiet -print_format json -show_format \"{path}\"";

            using var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = FFMpegPath + "/ffprobe.exe",
                    Arguments = $"-v quiet -print_format json -show_format \"{path}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            proc.Start();

            StringBuilder sb = new StringBuilder();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = await proc.StandardOutput.ReadLineAsync();
                sb.AppendLine(line);
            }

            string json = sb.ToString();

            /*
            Task<string> t = default;
            try
            {
                t = Probe.New().Start(strCmd);

                Task.WaitAny(t);
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message);
            }
            */

            // Console.WriteLine(videoInfo);

            JObject jo = JsonConvert.DeserializeObject<JObject>(json);

            // string[] formatArr = jo["format"]["format_name"].ToString()
            // .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (jo is null)
            {
                return string.Empty;
            }

            if (jo["format"]?["format_name"] is not null)
            {
                return jo["format"]["format_name"].ToString().ToLower();
            }

            return "";


            // return videoInfo;
        }
    }
}
