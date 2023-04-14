using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VStreamPlayer.MVVM.Views
{
    /// <summary>
    /// PlayListItemCollectionView.xaml 的交互逻辑
    /// </summary>
    public partial class PlayListItemCollectionView : UserControl, INotifyPropertyChanged
    {
        public List<PlayListItemView> Contents { get; set; } = new();

        /// <summary>
        /// Add an item and display it.
        /// </summary>
        /// <param name="view">Item</param>
        /// <returns>Add successfully or not.</returns>
        public bool AddItem(PlayListItemView view)
        {
            if (view is null)
            {
                return false;
            }

            if (Contents.Count != 0)
            {
                int max = Contents.Max(m => m.Order);
                view.Order = max + 1;
            }
            else
            {
                view.Order = 1;
            }

            Contents.Add(view);
            LstView.Items.Add(view);
            return true;
        }

        /// <summary>
        /// Clear all UI item elements.
        /// </summary>
        public void ClearItems()
        {
            LstView.Items.Clear();
            Contents.Clear();
        }

        private int _index = 0;
        
        public PlayListItemCollectionView()
        {
            InitializeComponent();

            // InitDatas();

            // LoadingDatas();
        }

        /// <summary>
        /// Refresh UI
        /// </summary>
        public void RefreshUi()
        {
            _index = 0;
            LstView.Items.Clear();
            foreach (var v in Contents)
            {
                v.Order = ++_index;
                LstView.Items.Add(v);
            }
        }

        private void LoadingDatas()
        {
            foreach (var v in Contents)
            {
                LstView.Items.Add(v);
            }
        }

        private void InitDatas()
        {
            PlayListItemView[] arrItem = new PlayListItemView[10];

            for (int i = 0; i < 10; i++)
            {
                
                arrItem[i] = new PlayListItemView(@"D:\马斯托鸡你太美.mp4") {Order = ++_index};
                
                // Contents.Add();
            }

            foreach (var item in arrItem)
            {
                Contents.Add(item);
            }

            // OnPropertyChanged("Contents");
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

        public void ResetVideosPlayOrder()
        {
            
        }

        /// <summary>
        /// Delete an item from Contents
        /// </summary>
        /// <param name="item">Specified item that should be deleted</param>
        public void DeleteItem(PlayListItemView item)
        {
            Contents.Remove(item);
            RefreshUi();
        }
    }
}
