using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VStreamPlayer.FFMpeg;

namespace VStreamPlayer.Core
{
    public class VideoInfo
    {
        // TODO: 这里用来定义分段播放属性
        // 例如 00:01:00~00:02:00 00:03:00~00:04:00...

        // TODO: 这里来定义视频详细信息属性
        // 例如：视频的编码方式、音频的编码方式...
        // public FileType FileType;

        private string _tmpFilePath;

        public double Volume
        {
            get { return MediaElement.Volume; }
            set { MediaElement.Volume = value; }
        }

        public string VideoName { get; private set; }

        /// <summary>
        /// Current video file has audio or not. <see langword="true" /> has audio. <see langword="false" /> does not have audio.
        /// </summary>
        public bool HasAudio { get; private set; }

        /// <summary>
        /// File extension name
        /// </summary>
        public string FileExtension { get; private set; }

        /// <summary>
        /// File actual extension name. Because user can modify the file extension name.
        /// </summary>
        public string FileActualExtension { get; private set; }

        // private Task _task;
        // private MediaElement _media;


        public MediaElement MediaElement { get; private set; }

        public VideoInfo(string videoPath)
        {
            VideoPath = videoPath;

            //_tmpFilePath = videoPath;
            //FileExtension = Path.GetExtension(videoPath).ToLower();
            //VideoName = Path.GetFileNameWithoutExtension(videoPath);

            /*
            try
            {
                CheckIsValidVideo(videoPath);
                // Task.WaitAny(CheckIsValidVideo(videoPath));
            }
            catch (NotSupportedException)
            {
                throw;
            }
            */

            /*
            MediaElement = new MediaElement
            {
                Source = new Uri(VideoPath),
                LoadedBehavior = MediaState.Manual
            };
            */

            // HasAudio = MediaElement.HasAudio;
            //IsReversePlay = false;
            //IsPlaying = false;
            //IsStopped = false;
            //IsPaused = false;
            //Volume = .5;
            //VideoCurrentProgress = TimeSpan.Zero;
            // VideoTotalDuration = MediaElement.NaturalDuration.HasTimeSpan ? MediaElement.NaturalDuration.TimeSpan : TimeSpan.Zero;
            //VideoRemainTime = VideoTotalDuration - VideoCurrentProgress;
        }

        /// <summary>
        /// Checking current video from path is valid video or not.
        /// </summary>
        /// <exception cref="NotSupportedException">Invalid video format</exception>
        private async void CheckIsValidVideo(string videoPath)
        {
            VideoPath = videoPath;

            #region Comments

            // Xabe.FFmpeg.FFmpeg.SetExecutablesPath("./ffmpeg/bin");

            // var fileType = detector.Extension;
            // FileType = detector.Extension.ToLower();//真实扩展名

            // string videoInfo = await Xabe.FFmpeg.Probe.New().Start($"-v quiet -print_format json -show_format {videoPath}");

            /*
            Process cmd = new Process();
            cmd.StartInfo.FileName = "systeminfo.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            Stream stream = cmd.StandardInput.BaseStream;

            string strCmd = $"probe -v quiet -print_format json -show_format {videoPath}";

            stream.Read(Encoding.UTF8.GetBytes(strCmd), 0, strCmd.Length);
            string videoInfo = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();
            */

            // JObject jo = JsonConvert.DeserializeObject<JObject>(videoInfo);
            #endregion

            string videoFormats = await FFMpegHelper.GetVideoFormat(videoPath);

            if (string.IsNullOrEmpty(videoFormats))
            {
                string str = new FrameworkElement().TryFindResource("NotSupportVideoFormat") as string;
                string message = str ?? "Invalid video format...";
                throw new NotSupportedException(message);
            }

            string[] formatArr = videoFormats
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            #region Comments
            // We can't find the video format, so this may not be a valid video file...
            /*if (formatArr is null || formatArr.Length == 0)
            {
                // Find local resource by key
                string str = new FrameworkElement().TryFindResource("NotSupportVideoFormat") as string;
                string message = str ?? "Invalid video format...";
                throw new NotSupportedException(message);
            }*/

            // We converted per formatArr element to lowercase.
            /*for (int i = 0; i < formatArr.Length; i++)
            {
                formatArr[i] = formatArr[i].ToLower();
            }*/


            /*bool isSupportVideo = false;

            bool isBreak = false;
            foreach (string format in formatArr)
            {
                if (!isBreak)
                {
                    foreach (var v in VideoEncodingType.VideoEncodingList)
                    {
                        if (v == format)
                        {
                            isSupportVideo = true;
                            isBreak = true;
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }*/
            #endregion

            if (string.IsNullOrEmpty(FileExtension))
            {
                
            }



            IsValidVideo = true;

            #region Comments
            /*
            if (!IsValidVideo)
            {
                // Find local resource by key
                string str = new FrameworkElement().TryFindResource("NotSupportVideoFormat") as string;
                string message = str ?? "Invalid video format...";
                throw new NotSupportedException(message);
            }

            if (string.IsNullOrEmpty(FileExtension))
            {
                VideoFileRename(formatArr[0]);
            }*/

            /*
            foreach (var tmp in formatArr)
            {
                if (FileExtension == tmp)
                {
                    FileActualExtension = FileExtension;
                    break;
                }
            }

            if (FileExtension != FileActualExtension && string.IsNullOrEmpty(FileActualExtension))
            {
                FileActualExtension = formatArr.First();
            }

            // Maybe FileExtension is "". That means current video file does not have extension name.
            // So we give it an extension name.

            if (string.IsNullOrEmpty(FileExtension))
            {
                if (!Directory.Exists("./temp"))
                {
                    Directory.CreateDirectory("./temp");
                }

                string videoTmpPath = $"./temp/{VideoName}.{FileActualExtension}";
                File.Copy(_tmpFilePath, videoTmpPath, true);


                // CheckIsValidVideo();
            }
            */
            #endregion
        }

        private void VideoFileRename(string fileFormat)
        {
            string newVideoFileName = "{VideoPath}.{fileFormat}";
            File.Copy(VideoPath, newVideoFileName);
            VideoPath = newVideoFileName;
            File.Delete(VideoPath);
        }

        /// <summary>
        /// Current video is reversed play or not. <see langword="true" /> is reversed play. <see langword="false" /> is not reversed play.
        /// </summary>
        public bool IsReversePlay { get; set; }

        /// <summary>
        /// Current video is valid video or not. <see langword="true"/> is valid. <see langword="false" /> is not valid.
        /// </summary>
        public bool IsValidVideo { get; set; }

        /// <summary>
        /// Actual video file type.
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Current video is playing or not. <see langword="true" /> is playing. <see langword="false" /> is not playing.
        /// </summary>
        public bool IsPlaying { get; set; }

        /// <summary>
        /// Current video is paused or not. <see langword="true" /> is paused. <see langword="false" /> is not paused.
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// Current video is stopped or not. <see langword="true" /> is stopped. <see langword="false" /> is not stopped.
        /// </summary>
        public bool IsStopped { get; set; }

        /// <summary>
        /// Video total duration.
        /// </summary>
        public TimeSpan VideoTotalDuration { get; set; }

        /// <summary>
        /// Video current progress.
        /// </summary>
        public TimeSpan VideoCurrentProgress { get; set; }

        /// <summary>
        /// Video remain time.
        /// </summary>
        public TimeSpan VideoRemainTime { get; set; }

        /// <summary>
        /// Video path.
        /// </summary>
        public string VideoPath { get; set; }

        // private CancellationTokenSource _cts = new CancellationTokenSource();

        public void Play()
        {
            MediaElement.Play();
            // _cts.Cancel();
            // _cts.Dispose();
            // _cts = new CancellationTokenSource();
            // UpdateTimeLineWhenPlaying(_cts);
            IsPlaying = true;
            IsPaused = false;
            IsStopped = false;
        }

        private void UpdateTimeLineWhenPlaying(CancellationTokenSource cts)
        {
            Task unused = Task.Run(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        VideoCurrentProgress = MediaElement.Position;
                        VideoRemainTime = VideoTotalDuration - VideoCurrentProgress;
                        if (VideoCurrentProgress == VideoTotalDuration)
                        {
                            break;
                        }
                        Thread.Sleep(100);
                    }
                }
            );
        }

        public void Pause()
        {
            MediaElement.Pause();
            // _cts.Cancel();
            // _cts.Dispose();
            IsPlaying = false;
            IsPaused = true;
            IsStopped = false;
        }

        public void Stop()
        {
            MediaElement.Stop();
            // _cts.Cancel();
            // _cts.Dispose();
            IsPlaying = false;
            IsPaused = false;
            IsStopped = true;
        }

        ~VideoInfo()
        {
            // Make sure that all resources are disposed of. 
            //MediaElement.Close();

            /*
            if (!_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
            */

            // _cts.Dispose();
            IsPlaying = false;
            IsPaused = false;
            IsStopped = false;
        }
    }
}
