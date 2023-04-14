using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using VStreamPlayer.Core;
using VStreamPlayer.MVVM.Models;
using VStreamPlayer.MVVM.ViewModels;
using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;
using Random = System.Random;

namespace VStreamPlayer.MVVM.Views
{
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
            PlayListItemsViewModel vm = CurrentPlayList.DataContext as PlayListItemsViewModel;
            try
            {
                CurrentPlay ??= vm.Contents.First();
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
                        var res = vm.Contents.SingleOrDefault(m => m.Order == (CurrentPlay.Order + 1));
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
                        NextPlay = vm.Contents.ElementAt(random.Next(0, vm.Contents.Count));
                    }
                    catch
                    {
                        // Maybe we can't find the element.
                        NextPlay = null;
                    }
                    break;
            }
        }

        private PlayListItemModel _currentPlay;

        /// <summary>
        /// Current play video.
        /// </summary>
        public PlayListItemModel CurrentPlay
        {
            get { return _currentPlay; }
            set
            {
                _currentPlay = value;
                SetPlayListContentsBackground(value);
            }
        }

        /// <summary>
        /// Next play video. <see langword="null" /> doesn't have the next play video.
        /// </summary>
        public PlayListItemModel NextPlay { get; set; }



        public PlayListItemCollectionViewNew PlayListContents { get; set; }
        public PlayListItemCollectionViewNew MyCollectionContents { get; set; }
        public PlayListItemCollectionViewNew PlayHistoryContents { get; set; }

        /// <summary>
        /// Current play list
        /// </summary>
        public PlayListItemCollectionViewNew CurrentPlayList { get; set; }

        public PlayListItemCollectionViewNew TmpContents { get; set; } = new();

        private string _searchText = "";

        // TODO: BUG: 这里可以做个 BUG，搜索功能无效。
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
            string strMyCollection = VStreamPlayer.Helper.Resource.FindStringResource("MyConnectionString", "My connection");
            string strPlayHistory = VStreamPlayer.Helper.Resource.FindStringResource("PlayHistoryString", "Play history");

            TabItem tab = PlayList.SelectedItem as TabItem;

            Label label = PlayList.SelectedContent as Label;

            if (string.IsNullOrEmpty(value))
            {
                if (tab.Header is string str && str == strPlayList)
                {
                    label.Content = PlayListContents;

                    var vm = PlayListContents.DataContext as PlayListItemsViewModel;
                    vm.RefreshConnection();
                }
                else if (tab.Header is string str2 && str2 == strMyCollection)
                {
                    label.Content = MyCollectionContents;

                    var vm = PlayListContents.DataContext as PlayListItemsViewModel;
                    vm.RefreshConnection();
                }
                else
                {
                    label.Content = strPlayHistory;

                    var vm = PlayListContents.DataContext as PlayListItemsViewModel;
                    vm.RefreshConnection();
                }
                return;
            }

            PlayListItemsViewModel vm2;

            if (tab.Header is string str1 && str1 == strPlayList)
            {
                vm2 = PlayListContents.DataContext as PlayListItemsViewModel;
                
            } 
            else if (tab.Header is string str2 && str2 == strMyCollection)
            {
                vm2 = MyCollectionContents.DataContext as PlayListItemsViewModel;
            }
            else
            {
                vm2 = PlayHistoryContents.DataContext as PlayListItemsViewModel;
            }

            IEnumerable<PlayListItemModel> res = from p in vm2.Contents
                where p.Title.Contains(value)
                select p;

            var tmpVm = TmpContents.DataContext as PlayListItemsViewModel;
            tmpVm.Contents.Clear();
            foreach (var item in res)
            {
                tmpVm.AddItem(item);
            }

            label.Content = TmpContents;
        }


        public PlayListView()
        {
            InitializeComponent();
            // DataContext = new PlayListViewModel();

            PlayListContents = new();
            MyCollectionContents = new();
            PlayHistoryContents = new();
            CurrentPlayList = PlayListContents;

            InitManually();
        }

        private void InitManually()
        {
            string strPlayList = VStreamPlayer.Helper.Resource.FindStringResource("PlayListString", "Play list");
            string strMyConnection = VStreamPlayer.Helper.Resource.FindStringResource("MyConnectionString", "My connection");
            string strPlayHistory = VStreamPlayer.Helper.Resource.FindStringResource("PlayHistoryString", "Play history");

            foreach (var v in PlayList.Items)
            {
                if (v is TabItem { Header: string str1 } item1 && str1 == strPlayList)
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
                else if (v is TabItem { Header: string str3 } item3 && str3 == strPlayHistory)
                {
                    if (item3.Content is Label label)
                    {
                        label.Content = PlayHistoryContents;
                    }
                }
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            // User clicked play button.
            if (e.OriginalSource is Button { Tag: "Play Button" } btn)
            {
                string strPlayList = VStreamPlayer.Helper.Resource.FindStringResource("PlayListString", "Play list");
                string strMyConnection = VStreamPlayer.Helper.Resource.FindStringResource("MyConnectionString", "My connection");
                string strPlayHistory = VStreamPlayer.Helper.Resource.FindStringResource("PlayHistoryString", "Play history");

                // Set Current play list.
                if (PlayList.SelectedItem is TabItem { Header: string str1 } && str1 == strPlayList)
                {
                    CurrentPlayList = PlayListContents;
                }
                else if (PlayList.SelectedItem is TabItem { Header: string str2 } && str2 == strMyConnection)
                {
                    CurrentPlayList = MyCollectionContents;
                }
                else if (PlayList.SelectedItem is TabItem { Header: string str3 } && str3 == strPlayHistory)
                {
                    CurrentPlayList = PlayHistoryContents;
                }

                // Get datacontext of button
                PlayListItemModel model = btn.DataContext as PlayListItemModel;

                // Let main page play the video.
                WindowCollection windows = Application.Current.Windows;
                foreach (var window in windows)
                {
                    if (window is MainWindow win)
                    {
                        await win.PlayAnotherVideo(model.VideoPath);
                    }
                }

                CurrentPlay = model;
                if (PlaybackOrder == PlaybackOrder.SingleLoop)
                {
                    NextPlay = model;
                }
                else if (PlaybackOrder == PlaybackOrder.RandomPlay)
                {
                    Random random = new Random();
                    try
                    {
                        var lstView = CurrentPlayList.Content as ListView;
                        var coll = lstView?.Items;

                        List<PlayListItemModel> list = new List<PlayListItemModel>();

                        foreach (var item in coll)
                        {

                            list.Add(item as PlayListItemModel);
                        }

                        int iMax = coll.Count;

                        NextPlay = list.ElementAt(random.Next(0, iMax));
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
                    var lstView = CurrentPlayList.Content as ListView;
                    var coll = lstView?.Items;

                    List<PlayListItemModel> list = new List<PlayListItemModel>();

                    foreach (var item in coll)
                    {

                        list.Add(item as PlayListItemModel);
                    }

                    int iIndex = model.Order;
                    iIndex += 1;
                    PlayListItemModel view = list.FirstOrDefault(select => select.Order == iIndex);
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
            else if (e.OriginalSource is Button {Tag: "Delete Button"} deleteBtn)
            {

                TabItem tabItem = PlayList.SelectedItem as TabItem;

                Label label = tabItem.Content as Label;

                // Get current play list.
                if (label?.Content is PlayListItemCollectionViewNew view)
                {
                    PlayListItemModel model = deleteBtn.DataContext as PlayListItemModel;

                    // TODO: BUG：这里可以用来做一个测试，刚开始时单击删除没有任何反应
                    var vm = view.DataContext as PlayListItemsViewModel;

                    var deletedList = new List<PlayListItemModel>();

                    deletedList.Add(model);

                    vm.DeleteItems(deletedList);

                    // Judge current play list is equals to deleted list.
                    if (CurrentPlayList == view)
                    {
                        // Judge NextPlay is existed or not. And we deal with it.
                        bool isEqualsZero = vm.Contents.Count == 0;

                        bool isNextPlayExisted = false;

                        if (!isEqualsZero)
                        {
                            isNextPlayExisted = vm.Contents.Contains(model);
                        }

                        if (!isNextPlayExisted)
                        {
                            if (isEqualsZero)
                            {
                                NextPlay = null;
                            }
                            else
                            {
                                NextPlay = vm.Contents.FirstOrDefault();
                            }
                        }
                    }
                }

            }


        }

        /// <summary>
        /// Set Play list items background.
        /// </summary>
        public void SetPlayListContentsBackground(PlayListItemModel view)
        {

            foreach (var item in PlayList.Items)
            {
                var tabItem = item as TabItem;

                SetSingleTabContentsBackground(tabItem);
            }


            view.Background = PlayListItemModel.PlayingBackground;

        }

        private void SetSingleTabContentsBackground(TabItem item)
        {
            Label label1 = item?.Content as Label;

            PlayListItemCollectionViewNew viewNew1 = label1?.Content as PlayListItemCollectionViewNew;

            ListView lstView1 = viewNew1?.Content as ListView;

            var models = lstView1.Items;

            foreach (var v in models)
            {
                var model = v as PlayListItemModel;
                model.Background = PlayListItemModel.DefaultBackground;
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

            // Stored the legal files.
            List<string> lstString = new List<string>();

            foreach (var str in strArr)
            {
                bool b = await VStreamPlayer.Core.HelperUtils.CheckIsValidVideo(str);
                lstString.Add(str);
            }

            TabItem label = PlayList.SelectedItem as TabItem;

            if (label?.Header is string strHeader)
            {
                string strPlayList = VStreamPlayer.Helper.Resource.FindStringResource("PlayListString", "Play list");
                string strMyConnection = VStreamPlayer.Helper.Resource.FindStringResource("MyConnectionString", "My connection");
                string strPlayHistory = VStreamPlayer.Helper.Resource.FindStringResource("PlayHistoryString", "Play history");

                PlayListItemCollectionViewNew view;

                if (strPlayList == strHeader)
                {
                    view = PlayListContents;
                }
                else if (strMyConnection == strHeader)
                {
                    view = MyCollectionContents;
                }
                else
                {
                    view = PlayHistoryContents;
                }

                foreach (var str in lstString)
                {
                    try
                    {
                        var item = new PlayListItemModel(str);

                        var vm = view.DataContext as PlayListItemsViewModel;

                        vm?.AddItem(item);
                    }
                    catch
                    {

                    }
                }

            }
            
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (PlayList.SelectedContent is Label label)
            {
                var view = label.Content as PlayListItemCollectionViewNew;

                var vm = view.DataContext as PlayListItemsViewModel;

                ListView lstView = view.Content as ListView;

                var items = lstView.SelectedItems;

                List<PlayListItemModel> deletedList = new List<PlayListItemModel>();

                if (items.Count > 0)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        deletedList.Add(items[i] as PlayListItemModel);
                    }

                    vm.DeleteItems(deletedList);

                    // bool isCurrentPlayExists = false;
                    bool isNextPlayExists = false;

                    isNextPlayExists = vm.Contents.Contains(NextPlay);

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
            // Set current play
            if (NextPlay is not null)
            {
                CurrentPlay = NextPlay;
            }


            var model = CurrentPlayList.DataContext as PlayListItemsViewModel;

            // Set next play
            switch (PlaybackOrder)
            {
                case PlaybackOrder.SequentialPlay:
                    var res = model.Contents.SingleOrDefault(m => m.Order == (CurrentPlay.Order + 1));
                    
                    NextPlay = res;
                    break;

                case PlaybackOrder.SingleLoop:
                    NextPlay = CurrentPlay;
                    break;

                case PlaybackOrder.RandomPlay:
                    Random random = new Random();
                    try
                    {
                        NextPlay = model.Contents.ElementAt(random.Next(0, model.Contents.Count));
                    }
                    catch
                    {
                        // Maybe we can't find the element.
                        NextPlay = null;
                    }
                    break;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveDatas();
        }

        private void SaveDatas()
        {
            string strPlayList = VStreamPlayer.Helper.Resource.FindStringResource("PlayListString", "Play list");
            string strMyConnection = VStreamPlayer.Helper.Resource.FindStringResource("MyConnectionString", "My connection");
            string strPlayHistory = VStreamPlayer.Helper.Resource.FindStringResource("PlayHistoryString", "Play history");

            /*
            foreach (var v in PlayList.Items)
            {
                // Save play list items.
                if (v is TabItem { Header: string str1 } item1 && str1 == strPlayList)
                {
                    string filePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Data");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    filePath = Path.Combine(filePath, "PlayList.xml");
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath);
                    }

                    XDocument doc = XDocument.Load(filePath);
                    var root = doc.Root;
                    if (root is null)
                    {
                        // XmlNode rootNode = doc.
                    }
                }
                // Save my connection items.
                else if (v is TabItem { Header: string str2 } item2 && str2 == strMyConnection)
                {
                    
                }
                // Save play history items.
                else if (v is TabItem { Header: string str3 } item3 && str3 == strPlayHistory)
                {
                    
                }
            }
            */
        }
    }
}