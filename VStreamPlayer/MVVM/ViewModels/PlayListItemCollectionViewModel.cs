using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VStreamPlayer.MVVM.Views;

namespace VStreamPlayer.MVVM.ViewModels
{
    public class PlayListItemCollectionViewModel
    {
        public ObservableCollection<PlayListItemView> Items { get; set; } = new();

        public PlayListItemCollectionViewModel()
        {
            Items.Add(new PlayListItemView(@"D:\马斯托鸡你太美.mp4"));
            Items.Add(new PlayListItemView(@"D:\马斯托鸡你太美.mp4"));
            Items.Add(new PlayListItemView(@"D:\马斯托鸡你太美.mp4"));
        }
    }
}
