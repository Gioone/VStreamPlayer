using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using VStreamPlayer.Core;
using VStreamPlayer.MVVM.Views;
using VStreamPlayer.Window.MessageBox;
using Application = System.Windows.Application;
using WindowState = System.Windows.WindowState;


namespace VStreamPlayer
{
    // TODO: 打开视频添加到播放列表的功能还未实现，拖拽打开视频功能还未实现
    // TODO: 鼠标在进度条上时，应该显示视频的缩略图
    // TODO: 打开视频，音量的大小等操作应该显示在窗体上给用户提示
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Path of current playing video.
        /// </summary>
        public string CurrentVideoPath { get; set; }

        /// <summary>
        /// Play List
        /// </summary>
        private static PlayListView PlayList { get; set; } = new();

        /// <summary>
        /// Settings
        /// </summary>
        private static SettingsView SettingsView { get; set; } = new();

        private PlaybackOrder _playbackOrder = PlaybackOrder.SequentialPlay;

        /// <summary>
        /// Video playback order
        /// </summary>
        public PlaybackOrder PlaybackOrder
        {
            get { return _playbackOrder; }
            set
            {
                SetField(ref _playbackOrder, value);
                PlayList.PlaybackOrder = _playbackOrder;
            }
        }

        /// <summary>
        /// Current MainWindow state
        /// </summary>
        public VStreamPlayer.Core.WindowState VStreamPlayerWindowState { get; set; } = Core.WindowState.Normal;

        // private bool _currentVideoIsValid;

        public bool CurrentVideoIsValid
        {
            get;
            set;
        }

        private bool _isPaused;

        /// <summary>
        /// Current video is paused or not. <see langword="true" /> is paused. <see langword="false" /> is not paused.
        /// </summary>
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                SetField(ref _isPaused, value);

                if (_isPaused)
                {
                    IsPlaying = false;
                    IsStopped = false;

                    StopTimer();
                }
            }
        }

        private bool _isPlaying;

        /// <summary>
        /// Current video is playing or not. <see langword="true" /> is playing. <see langword="false" /> is not playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                SetField(ref _isPlaying, value);
                if (_isPlaying)
                {
                    BtnPlayAndPause.IsChecked = true;
                    IsPaused = false;
                    IsStopped = false;

                    UpdateProgressBar();
                }
                else
                {
                    BtnPlayAndPause.IsChecked = false;
                }

                SetVideoTitle();
            }
        }

        private void SetVideoTitle()
        {
            if (File.Exists(CurrentVideoPath))
            {
                FileInfo fi = new FileInfo(CurrentVideoPath);

                string[] arr = fi.Name.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);

                TxtVideoTitle.Text = arr.Length == 2 ? arr[0] : fi.Name;
            }
        }

        DispatcherTimer _timer;

        /// <summary>
        /// Update video progress bar
        /// </summary>
        private void UpdateProgressBar()
        {
            // Stop last timer.
            if (_timer is not null)
            {
                _timer.Stop();
                _timer.IsEnabled = false;
            }


            // SetDuration
            string strClock = FFMpeg.FFMpegHelper.GetVideoDurationClock(CurrentVideoPath);
            TxtLastTime.Text = strClock;
            ProgressSlider.Minimum = 0;
            ProgressSlider.Maximum = VStreamPlayer.Helper.Convert.ClockToMilliseconds(strClock);

            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(10),
                IsEnabled = true
            };

            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            double ticks = MediaMain.Position.TotalMilliseconds;

            long totals;
            try
            {
                totals = System.Convert.ToInt64(ticks);
            }
            catch (OverflowException)
            {
                totals = 0;
            }

            string strClock = VStreamPlayer.Helper.Convert.MillisecondsToClock(totals);

            TxtCurrentTime.Text = strClock;
            ProgressSlider.Value = ticks;
        }

        private bool _isStopped;

        /// <summary>
        /// Current video is stopped or not. <see langword="true" /> is stopped. <see langword="false" /> is not stopped.
        /// </summary>
        public bool IsStopped
        {
            get { return _isStopped; }
            set
            {
                SetField(ref _isStopped, value);

                if (_isStopped)
                {
                    IsPlaying = false;
                    IsPaused = false;

                    StopTimer();
                }
            }
        }

        private void StopTimer()
        {
            if (_timer is not null)
            {
                _timer.Stop();
                _timer.IsEnabled = false;
            }
        }

        private VideoInfo _videoInfo;

        public VideoInfo VideoInfo
        {
            get
            {
                return _videoInfo;
            }

            set
            {
                SetField(ref _videoInfo, value);
            }
        }

        private bool _isOnTop;

        /// <summary>
        /// A normal property. Current window is on-top or not. <see langword="true" /> is on-top. <see langword="false" /> is not on-top.
        /// </summary>
        public bool IsOnTop
        {
            get
            {
                return _isOnTop;
            }

            set
            {
                SetField(ref _isOnTop, value);
            }
        }

        static MainWindow()
        {

        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Minimized)
            {
                WindowState = WindowState.Minimized;
            }
        }


        private void MediaMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
#if false
            // Point cursorPosition = e.GetPosition(MediaMain);

            /*
            if ((cursorPosition.X >= 0 && cursorPosition.X <= MediaMain.ActualWidth) && cursorPosition.Y >= 0 &&
                cursorPosition.Y <= MediaMain.ActualHeight)
            {

            }
            */

            #region comment
            /*
            if (_firstMouseMoveTime is null)
            {
                _firstMouseMoveTime = DateTime.Now;
                _firstMousePosition = PointToScreen(e.GetPosition(MediaMain));
            }
            else
            {
                _lastMouseMoveTime = DateTime.Now;
                _lastMousePosition = PointToScreen(e.GetPosition(MediaMain));
            }

            if (_firstMousePosition is not null && _lastMousePosition is not null)
            {
                if (_firstMousePosition != _lastMousePosition && e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                    _firstMouseMoveTime = _lastMouseMoveTime = null;
                }
            }
            */

            /*
            _timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromMilliseconds(200),
                IsEnabled = false
            };
            _timer.Tick += Timer_Tick;

            if (_timer is {IsEnabled: false})
            {
                _timer.IsEnabled = true;
            }
            */
            #endregion
#endif
            /*
            // Pressed to drag and move.
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //if (WindowState == WindowState.Maximized)
                //{
                //    WindowState = WindowState.Normal;
                //}

                DragMove();
            }

            // Double click to play or pause.
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                // VStreamPlayer.Commands.PlayCommand.Command

                if (CheckVideoStatus())
                {
                    PlayOrPauseVideo();
                }
            }
            */

        }

        /// <summary>
        /// If  the video is playing, then pauses the video. If  the  video is paused, then play the video.
        /// </summary>
        private void PlayOrPauseVideo()
        {
            if (IsPlaying)
            {
                if (MediaMain.CanPause)
                {
                    MediaMain.Pause();
                    IsPaused = true;
                }
            }
            else if (IsPaused)
            {
                MediaMain.Play();
                IsPlaying = true;
            }
            else if (IsStopped)
            {
                MediaMain.Play();
                IsPlaying = true;
            }

        }

        /// <summary>
        /// Set Window on-top.
        /// </summary>
        private void SetWindowOnTop()
        {
            if (IsOnTop)
            {
                Topmost = false;

                IsOnTop = false;
            }
            else
            {
                Topmost = true;
                IsOnTop = true;
            }
        }

        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            MaximizeOrNormalWindow();
        }

        private void MaximizeOrNormalWindow()
        {
            if (VStreamPlayerWindowState == Core.WindowState.Normal ||
                VStreamPlayerWindowState == Core.WindowState.BorderlessNormal)
            {
                ChangeWindowState(Core.WindowState.BorderlessMaximized);
            }
            else
            {
                ChangeWindowState(Core.WindowState.Normal);
            }
            /*
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                // LayoutArea.Margin = new Thickness(8);
            }
            else if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                // LayoutArea.Margin = new Thickness(0);
            }
            */
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // When the window size has changed to maximized. We need to reset border thickness for the window.
            // Otherwise, not all content will be displayed.

            if (WindowState == WindowState.Maximized)
            {
                LayoutArea.Margin = LayoutArea.Margin = new Thickness(8);
            }
            else
            {
                LayoutArea.Margin = LayoutArea.Margin = new Thickness(0);
            }
        }

        private async void Logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


            if (e.LeftButton == MouseButtonState.Pressed)
            {
                /*
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                }
                */

                DragMove();
            }


            // Double click to open video file.
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                string videoPath = HelperUtils.OpenSingleVideo();
                if (string.IsNullOrEmpty(videoPath))
                {
                    return;
                }

                try
                {
                    // First, let's check current video from path is a valid video or not.
                    bool isValidVideo = await HelperUtils.CheckIsValidVideo(videoPath);
                    CurrentVideoIsValid = isValidVideo;


                    string title = VStreamPlayer.Helper.Resource.FindStringResource("Information", "Information");
                    string text = VStreamPlayer.Helper.Resource.FindStringResource("CouldNotOpenVideoFileString", "Sorry, we could not open this file.It seems to be an invalid video file...");
                    string confirm = VStreamPlayer.Helper.Resource.FindStringResource("ConfirmString", "Confirm");

                    if (!isValidVideo)
                    {
                        MessageBoxInfo info = new MessageBoxInfo(title, text, confirm);
                        info.ShowDialog();

                        return;
                    }


                    if (CurrentVideoIsValid)
                    {
                        // We hide this element under the MediaMain element. 
                        Logo.SetValue(Panel.ZIndexProperty, -1);
                        MediaMain.Source = new Uri(videoPath);
                        CurrentVideoPath = videoPath;

                        try
                        {
                            var item = new PlayListItemView(videoPath);
                            PlayList.PlayListContents.AddItem(item);
                            PlayList.CurrentPlayList = PlayList.PlayListContents;
                            PlayList.SetPlayListContentsBackground(item);
                            PlayList.CurrentPlay = item;
                            PlayList.SetNextPlayVideo();
                        }
                        catch
                        {

                        }

                        MediaMain.Play();
                        IsPlaying = true;
                    }
                    else
                    {
                        MessageBoxInfo info = new MessageBoxInfo(title, text, confirm);
                        info.ShowDialog();
                    }
                    // TODO: 没有后缀名的文件没有做处理
                }
                catch (NotSupportedException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void TglBtnOnTop_Click(object sender, RoutedEventArgs e)
        {
            SetWindowOnTop();
        }

        private void BtnPlayAndPause_Click(object sender, RoutedEventArgs e)
        {
            if (CheckVideoStatus())
            {
                PlayOrPauseVideo();
            }
        }

        /// <summary>
        /// Check video source is valid or not. <see langword="true" /> is valid. <see langword="false" /> is invalid.
        /// </summary>
        /// <returns><see langword="true" /> is valid. <see langword="false" /> is invalid.</returns>
        private bool CheckVideoStatus()
        {
            if (!IsStopped && !IsPaused && !IsPlaying)
            {
                BtnPlayAndPause.IsChecked = false;
                return false;
            }

            return true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Space)
            {
                if (CheckVideoStatus())
                {
                    PlayOrPauseVideo();
                }
            }
        }

        private void MediaMain_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            IsPlaying = false;
            IsPaused = false;
            IsStopped = false;
            CurrentVideoIsValid = false;
        }

        private void MediaMain_MediaOpened(object sender, RoutedEventArgs e)
        {
            CurrentVideoIsValid = true;
            // 通知 PlayList 为下一首播放的歌曲做准备。
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // new SettingsView().Show();
        }

        private void MediaMain_MediaEnded(object sender, RoutedEventArgs e)
        {
            PlayNextVideo();
        }

        private async void PlayNextVideo()
        {
            MediaMain.Stop();
            IsStopped = true;

            var nextPlay = PlayList.NextPlay;

            if (nextPlay is null)
            {
                return;
            }

            await PlayAnotherVideo(nextPlay.VideoPath);

            PlayList.SetCurrentAndNextPlayVideo();
            PlayList.SetPlayListContentsBackground(PlayList.CurrentPlay);
        }

        private bool _isContinueToPlay;

        private void MediaSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (!CheckVideoStatus())
            {
                return;
            }

            if (IsPlaying)
            {
                PlayOrPauseVideo();
                _isContinueToPlay = true;
            }
        }

        private void MediaSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (!CheckVideoStatus())
            {
                return;
            }

            if (IsPaused)
            {

                if (e.Source is Slider slider)
                {
                    TimeSpan value = TimeSpan.FromMilliseconds(slider.Value);
                    MediaMain.Position = value;
                    if (_isContinueToPlay)
                    {
                        PlayOrPauseVideo();
                        _isContinueToPlay = false;
                    }
                }

            }
        }


        public async Task PlayAnotherVideo(string path)
        {
            bool isValid = await HelperUtils.CheckIsValidVideo(path);
            CurrentVideoPath = path;
            CurrentVideoIsValid = isValid;
            if (!isValid)
            {
                return;
            }

            Logo.SetValue(Panel.ZIndexProperty, -1);

            StopVideo();
            MediaMain.Source = new Uri(path);
            MediaMain.Play();
            IsPlaying = true;
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // ProgressSlider.Value = e.NewValue;
            // MediaMain.Position = TimeSpan.FromMilliseconds(e.NewValue);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            StopVideo();
        }

        private void StopVideo()
        {
            IsStopped = true;
            MediaMain.Stop();
        }

        private void PlayArea_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Pressed to drag and move.
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //if (WindowState == WindowState.Maximized)
                //{
                //    WindowState = WindowState.Normal;
                //}

                DragMove();
            }

            // Double click to play or pause.
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                // VStreamPlayer.Commands.PlayCommand.Command

                if (CheckVideoStatus())
                {
                    PlayOrPauseVideo();
                }
            }
        }

        private void BtnWindowed_Click(object sender, RoutedEventArgs e)
        {
            switch (VStreamPlayerWindowState)
            {
                // The current state of the window is the normal state,
                // we need to change it to the borderless state
                case Core.WindowState.Normal:
                    ChangeWindowState(Core.WindowState.BorderlessNormal);
                    break;

                // The current state of the window is maximized with a border,
                // we need to change its state to maximized state
                case Core.WindowState.BorderMaximized:
                    ChangeWindowState(Core.WindowState.BorderlessMaximized);
                    break;

                // The current state of the window is borderless normal state,
                // we need to change its state to normal with a border state
                case Core.WindowState.BorderlessNormal:
                    ChangeWindowState(Core.WindowState.Normal);
                    break;

                // The current state of the window is maximized without border,
                // we need to change its state to maximized with border
                case Core.WindowState.BorderlessMaximized:
                    ChangeWindowState(Core.WindowState.BorderMaximized);
                    break;

                // Unknown state... We do nothing.
                default:
                    break;
            }
        }

        /// <summary>
        /// Change current MainWindow window state.
        /// </summary>
        /// <param name="state">Window state</param>
        private void ChangeWindowState(Core.WindowState state)
        {
            switch (state)
            {
                // Normal state
                case Core.WindowState.Normal:
                    GridWindowTitle.SetValue(Grid.RowProperty, 0);
                    GridWindowTitle.Visibility = Visibility.Visible;

                    Logo.SetValue(Grid.RowProperty, 1);
                    Logo.SetValue(Grid.RowSpanProperty, 1);
                    PlayArea.SetValue(Grid.RowProperty, 1);
                    PlayArea.SetValue(Grid.RowSpanProperty, 1);

                    GridFunctionality.SetValue(Grid.RowProperty, 2);
                    GridFunctionality.Visibility = Visibility.Visible;

                    WindowState = WindowState.Normal;
                    VStreamPlayerWindowState = Core.WindowState.Normal;
                    break;

                // Maximized with the border state
                case Core.WindowState.BorderMaximized:
                    GridWindowTitle.SetValue(Grid.RowProperty, 0);
                    GridWindowTitle.Visibility = Visibility.Visible;

                    Logo.SetValue(Grid.RowProperty, 1);
                    Logo.SetValue(Grid.RowSpanProperty, 1);
                    PlayArea.SetValue(Grid.RowProperty, 1);
                    PlayArea.SetValue(Grid.RowSpanProperty, 1);

                    GridFunctionality.SetValue(Grid.RowProperty, 2);
                    GridFunctionality.Visibility = Visibility.Visible;


                    WindowState = WindowState.Maximized;
                    VStreamPlayerWindowState = Core.WindowState.BorderMaximized;
                    break;

                case Core.WindowState.BorderlessNormal:
                    GridWindowTitle.SetValue(Grid.RowProperty, 0);
                    GridWindowTitle.Visibility = Visibility.Hidden;

                    Logo.SetValue(Grid.RowProperty, 0);
                    Logo.SetValue(Grid.RowSpanProperty, 3);

                    PlayArea.SetValue(Grid.RowProperty, 0);
                    PlayArea.SetValue(Grid.RowSpanProperty, 3);

                    GridFunctionality.SetValue(Grid.RowProperty, 2);
                    GridFunctionality.Visibility = Visibility.Hidden;

                    WindowState = WindowState.Normal;
                    VStreamPlayerWindowState = Core.WindowState.BorderlessNormal;
                    break;

                case Core.WindowState.BorderlessMaximized:
                    GridWindowTitle.SetValue(Grid.RowProperty, 0);
                    GridWindowTitle.Visibility = Visibility.Hidden;

                    Logo.SetValue(Grid.RowProperty, 0);
                    Logo.SetValue(Grid.RowSpanProperty, 3);

                    PlayArea.SetValue(Grid.RowProperty, 0);
                    PlayArea.SetValue(Grid.RowSpanProperty, 3);

                    GridFunctionality.SetValue(Grid.RowProperty, 2);
                    GridFunctionality.Visibility = Visibility.Hidden;

                    WindowState = WindowState.Maximized;
                    VStreamPlayerWindowState = Core.WindowState.BorderlessMaximized;
                    break;
            }
        }

        /// <summary>
        /// In the window without border state, we need to display or hide the UI controls
        /// </summary>
        private void DisplayOrHiddenControls()
        {
            if (VStreamPlayerWindowState is Core.WindowState.BorderlessNormal or Core.WindowState.BorderlessMaximized)
            {
                Point position = Mouse.GetPosition(this);

                // Display title area.
                if (position.Y <= GridWindowTitle.ActualHeight)
                {
                    GridWindowTitle.Visibility = Visibility.Visible;
                }
                else
                {
                    GridWindowTitle.Visibility = Visibility.Hidden;
                }

                double dNonFunctionalityHeight = ActualHeight - GridFunctionality.ActualHeight;

                // Display functionality area.
                if (position.Y >= (dNonFunctionalityHeight - 10))
                {
                    GridFunctionality.Visibility = Visibility.Visible;
                }
                else
                {
                    GridFunctionality.Visibility = Visibility.Hidden;
                }

                // Debug.WriteLine(GridWindowTitle.ActualHeight);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            DisplayOrHiddenControls();
        }

        private void GridWindowTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }


            // Double click to change window state.
            if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
            {
                if (VStreamPlayerWindowState is Core.WindowState.Normal or Core.WindowState.BorderlessNormal)
                {
                    ChangeWindowState(Core.WindowState.BorderMaximized);
                }
                else if (VStreamPlayerWindowState is Core.WindowState.BorderMaximized or Core.WindowState.BorderlessMaximized)
                {
                    ChangeWindowState(Core.WindowState.Normal);
                }
            }
        }

        private void BtnMultiplyPlayBackward_Click(object sender, RoutedEventArgs e)
        {
            if (MediaMain.SpeedRatio <= 0.25)
            {
                return;
            }

            double dCurrentSpeed = MediaMain.SpeedRatio;
            dCurrentSpeed -= 0.25;

            MediaMain.SpeedRatio = dCurrentSpeed;
        }

        private void BtnMultiplyPlayBackward_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MediaMain.SpeedRatio = 1.0;
        }

        private void BtnMultiplyPlayForward_Click(object sender, RoutedEventArgs e)
        {
            if (MediaMain.SpeedRatio >= 5.0)
            {
                return;
            }

            double dCurrentSpeed = MediaMain.SpeedRatio;
            dCurrentSpeed += 0.25;

            MediaMain.SpeedRatio = dCurrentSpeed;
        }

        private void BtnMultiplyPlayForward_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MediaMain.SpeedRatio = 1.0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PlayList.Show();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // TODO: 这里用来做视频进度条的点击跳转到对应位置
            // Point point = e.GetPosition(ProgressSlider);
            // Debug.WriteLine($"{point.X}, {point.Y}");
        }

        private void BtnPlaybackOrder_Click(object sender, RoutedEventArgs e)
        {
            ChangePlaybackOrder();
        }

        /// <summary>
        /// Change video playback order.
        /// </summary>
        private void ChangePlaybackOrder()
        {
            PlaybackOrder = PlaybackOrder switch
            {
                PlaybackOrder.SequentialPlay => PlaybackOrder.SingleLoop,
                PlaybackOrder.SingleLoop => PlaybackOrder.RandomPlay,
                PlaybackOrder.RandomPlay => PlaybackOrder.SequentialPlay,
                _ => PlaybackOrder
            };
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsView.Show();
        }

        private static AboutView aboutView = new();

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            aboutView.Show();
        }
    }
}
