using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VStreamPlayer.MVVM.Models;

namespace VStreamPlayer.MVVM.ViewModels
{
    public class PlayListItemsViewModel
    {
        private int _iIndex;

        public ObservableCollection<PlayListItemModel> Contents { get; set; } = new();

        // public List<PlayListItemModel> Contents { get; set; } = new List<PlayListItemModel>();

        public PlayListItemsViewModel()
        {
            InitTestDatas();
        }

        private void InitTestDatas()
        {
            /*
            for (int i = 0; i < 10; i++)
            {
                AddItem(new PlayListItemModel(@"D:\马斯托鸡你太美.mp4"));
            }
            */
        }

        public bool DeleteItems(List<PlayListItemModel> models)
        {
            if (models is null)
            {
                return false;
            }

            for (int i = 0; i < models.Count; i++)
            {
                Contents.Remove(models[i]);
            }

            RefreshConnection();

            return true;
        }

        public void RefreshConnection()
        {
            _iIndex = 0;
            for (int i = 0; i < Contents.Count; i++)
            {
                Contents[i].Order = ++_iIndex;
            }
        }

        public bool AddItem(PlayListItemModel model)
        {
            if (model is null)
            {
                return false;
            }

            if (Contents.Count != 0)
            {
                int max = Contents.Max(m => m.Order);
                model.Order = max + 1;
            }
            else
            {
                model.Order = 1;
            }

            Contents.Add(model);


            return true;
        }

    }
}
