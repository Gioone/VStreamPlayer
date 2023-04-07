using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VStreamPlayer.Core;
using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;
using Random = System.Random;

namespace VStreamPlayer.MVVM.Views
{
    // TODO: 歌曲单项的删除按钮可以优化一下
    // TODO: 播放列表和我的收藏需要保存到本地磁盘的介质中
    /// <summary>
    /// PlayListView.xaml 的交互逻辑
    /// </summary>
    public partial class PlayListView : System.Windows.Window, INotifyPropertyChanged
    {
        private PlaybackOrder _playbackOrder = PlaybackOrder.SequentialPlay;

        public PlaybackOrder PlaybackOrder
        {
            get
            {
                return _playbackOrder;
            }

            set
            {
                if (value != _playbackOrder)
                {
                    _playbackOrder = value;
                    // Because user has changed play order, so we should reset next play video.
                    SetNextPlayVideo();
                }
            }
        }

        /// <summary>
        /// Set next play video
        /// </summary>
        public void SetNextPlayVideo()
        {
            try
            {
                CurrentPlay ??= CurrentPlayList.Contents.First();
            }
            catch
            {
                CurrentPlay = null;
            }

            switch (PlaybackOrder)
            {
                case PlaybackOrder.SequentialPlay:
                    if (CurrentPlay is null)
                    {
                        NextPlay = null;
                    }
                    else
                    {
                        var res = CurrentPlayList.Contents.SingleOrDefault(m => m.Order == (CurrentPlay.Order + 1));
                        NextPlay = res;
                    }

                    break;

                case PlaybackOrder.SingleLoop:
                    NextPlay = CurrentPlay;
                    break;

                case PlaybackOrder.RandomPlay:
                    Random random = new Random();
                    try
                    {
                        NextPlay = CurrentPlayList.Contents.ElementAt(random.Next(0, CurrentPlayList.Contents.Count));
                    }
                    catch
                    {
                        // Maybe we can't find the element.
                        NextPlay = null;
                    }
                    break;
            }
        }

        /// <summary>
        /// Current play video.
        /// </summary>
        public PlayListItemView CurrentPlay { get; set; }

        /// <summary>
        /// Next play video. <see langword="null" /> doesn't have the next play video.
        /// </summary>
        public PlayListItemView NextPlay { get; set; }



        public PlayListItemCollectionView PlayListContents { get; set; }
        public PlayListItemCollectionView MyCollectionContents { get; set; }
        public PlayListItemCollectionView LocalDiskContents { get; set; }

        /// <summary>
        /// Current play list
        /// </summary>
        public PlayListItemCollectionView CurrentPlayList { get; set; }

        private string _searchText = "";

        // TODO: 搜索功能无效，应该优化
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (SetField(ref _searchText, value))
                {
                    SelectVideos(value);
                }
            }
        }

        private void SelectVideos(string value)
        {
            string strPlayList = VStreamPlayer.Helper.Resource.FindStringResource("PlayListString", "Play list");
            if (PlayList.SelectedItem is string str && str == strPlayList)
            {
                if ((PlayList.SelectedContent as ScrollViewer)?.Content is PlayListItemCollectionView content)
                {
                    IEnumerable enumerable = content.Contents.Select(e => e.Title.Contains(value));
                    foreach (var v in enumerable)
                    {
                        var i = v as PlayListItemView;
                    }
                }
            }
        }


        public PlayListView()
        {
            InitializeComponent();
            // DataContext = new PlayListViewModel();

            PlayListContents = new();
            MyCollectionContents = new();
            LocalDiskContents = new();
            CurrentPlayList = PlayListContents;

            InitManually();
        }

        private void InitManually()
        {
            string strPlayList = VStreamPlayer.Helper.Resource.FindStringResource("PlayListString", "Play list");
            string strMyConnection = VStreamPlayer.Helper.Resource.FindStringResource("MyConnectionString", "My connection");
            string strLocalDisk = VStreamPlayer.Helper.Resource.FindStringResource("LocalDiskString", "Local disk");

            foreach (var v in PlayList.Items)
            {
                if (v is TabItem {Header: string str1} item1 && str1 == strPlayList)
                {
                    if (item1.Content is Label label)
                    {
                        label.Content = PlayListContents;
                    }
                }
                else if (v is TabItem { Header: string str2 } item2 && str2 == strMyConnection)
                {
                    if (item2.Content is Label label)
                    {
                        label.Content = MyCollectionContents;
                    }
                } 
                else if(v is TabItem { Header: string str3 } item3 && str3 == strLocalDisk)
                {
                    if (item3.Content is Label label)
                    {
                        label.Content = LocalDiskContents;
                    }
                }
            }

            /*
            if (this.PlayList.SelectedItem is TabItem item)
            {
                if (item.Header is string str && str == strPlayList)
                {
                    // ScrollView

                    // PlayListContents = new PlayListItemCollectionView();

                    if (PlayList.SelectedContent is Label obj) obj.Content = PlayListContents;
                }
            }*/
        }

        private void InitTestDatas()
        {
#if DEBUG
            string strPlayList = VStreamPlayer.Helper.Resource.FindStringResource("PlayListString", "Play list");
            if (this.PlayList.SelectedItem is TabItem item)
            {
                if (item.Header is string str && str == strPlayList)
                {
                    // ScrollView
                    var obj = PlayList.SelectedContent as Label;

                    PlayListContents = new PlayListItemCollectionView();

                    if (obj is not null) obj.Content = PlayListContents;
                }
            }
#endif
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // User clicked play button.
            if (e.OriginalSource is Button { Tag: "Play Button" } btn)
            {
                string strPlayList = VStreamPlayer.Helper.Resource.FindStringResource("PlayListString", "Play list");
                string strMyConnection = VStreamPlayer.Helper.Resource.FindStringResource("MyConnectionString", "My connection");
                string strLocalDisk = VStreamPlayer.Helper.Resource.FindStringResource("LocalDiskString", "Local disk");

                // Set Current play list.
                if (PlayList.SelectedItem is string str1 && str1 == strPlayList)
                {
                    CurrentPlayList = PlayListContents;
                } 
                else if (PlayList.SelectedItem is string str2 && str2 == strMyConnection)
                {
                    CurrentPlayList = MyCollectionContents;
                }
                else if (PlayList.SelectedItem is string str3 && str3 == strLocalDisk)
                {
                    CurrentPlayList = LocalDiskContents;
                }

                // Find the parent control which is named PlayListItemView.
                PlayListItemView item = VStreamPlayer.Core.HelperUtils.GetParentControl<PlayListItemView>(btn);

                SetPlayListContentsBackground(item);


                CurrentPlay = item;
                if (PlaybackOrder == PlaybackOrder.SingleLoop)
                {
                    NextPlay = item;
                }
                else if (PlaybackOrder == PlaybackOrder.RandomPlay)
                {
                    Random random = new Random();
                    try
                    {
                        NextPlay = CurrentPlayList.Contents.ElementAt(random.Next(0, CurrentPlayList.Contents.Count));
                    }
                    catch
                    {
                        // Maybe we can't find the element.
                        NextPlay = null;
                    }
                }
                // Sequential play
                else
                {
                    int iIndex = item.Order;
                    PlayListItemView view = CurrentPlayList.Contents.SingleOrDefault(select => select.Order == ++iIndex);
                    if (view is not null)
                    {
                        NextPlay = view;
                    }
                    else
                    {
                        NextPlay = null;
                    }
                }
            }
        }

        /// <summary>
        /// Set Play list items background.
        /// </summary>
        public void SetPlayListContentsBackground(PlayListItemView view)
        {
            foreach (var v in PlayListContents.Contents)
            {
                v.Background = VStreamPlayer.Helper.Resource.FindColorResource("SecondaryColorBrush");
            }

            foreach (var v in MyCollectionContents.Contents)
            {
                v.Background = VStreamPlayer.Helper.Resource.FindColorResource("SecondaryColorBrush");
            }

            foreach (var v in LocalDiskContents.Contents)
            {
                v.Background = VStreamPlayer.Helper.Resource.FindColorResource("SecondaryColorBrush");
            }
            if (view is not null)
            {
                // TODO: 自动播放下一首时，当前播放的歌曲背景色没有得到更改
                Brush brush = VStreamPlayer.Helper.Resource.FindColorResource("MainColorBrush");
                view.Background = brush;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // InitTestDatas();
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string[] strArr = VStreamPlayer.Core.HelperUtils.OpenMultiVideos();
            if (strArr is null || strArr.Length == 0) return;

            List<string> lstString = new List<string>();

            foreach (var str in strArr)
            {
                bool b = await VStreamPlayer.Core.HelperUtils.CheckIsValidVideo(str);
                lstString.Add(str);
            }

            Label label = PlayList.SelectedContent as Label;

            if (label?.Content is PlayListItemCollectionView view)
            {
                foreach (var str in lstString)
                {
                    try
                    {
                        var item = new PlayListItemView(str);

                        // Get the max order.
                        int iMax = view.Contents.Max(m => m.Order);

                        // PlayListItemView res = view.Contents.SingleOrDefault(m => m.Order == iMax);

                        item.Order = ++iMax;

                        view.Contents.Add(item);
                    }
                    catch
                    {

                    }
                }

                view.RefreshUi();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (PlayList.SelectedContent is Label label)
            {
                ListView lstView = VStreamPlayer.Core.HelperUtils.GetChildControl<ListView>(label);

                var items = lstView.SelectedItems;

                var view = VStreamPlayer.Core.HelperUtils.GetChildControl<PlayListItemCollectionView>(label);

                if (items.Count > 0)
                {
                    foreach (var v in items)
                    {
                        var removed = v as PlayListItemView;
                        view.Contents.Remove(removed);
                    }

                    view.RefreshUi();

                    // bool isCurrentPlayExists = false;
                    bool isNextPlayExists = false;

                    isNextPlayExists = view.Contents.Contains(NextPlay);

                    if (!isNextPlayExists)
                    {
                        // We can't find the next play video, because user has deleted it.
                        NextPlay = null;
                    }


                }

            }
        }

        /// <summary>
        /// Set current and next play video.
        /// </summary>
        public void SetCurrentAndNextPlayVideo()
        {
            CurrentPlay = NextPlay;
            switch (PlaybackOrder)
            {
                case PlaybackOrder.SequentialPlay:
                    var res = CurrentPlayList.Contents.SingleOrDefault(m => m.Order == (CurrentPlay.Order + 1));
                    if (res is not null)
                    {
                        NextPlay = res;
                    }

                    // Currently at the end of the playlist.
                    if (res is null)
                    {
                        NextPlay = null;
                    }
                    break;

                case PlaybackOrder.SingleLoop:
                    NextPlay = CurrentPlay;
                    break;

                case PlaybackOrder.RandomPlay:
                    Random random = new Random();
                    try
                    {
                        NextPlay = CurrentPlayList.Contents.ElementAt(random.Next(0, CurrentPlayList.Contents.Count));
                    }
                    catch
                    {
                        // Maybe we can't find the element.
                        NextPlay = null;
                    }
                    break;
            }
        }
    }
}