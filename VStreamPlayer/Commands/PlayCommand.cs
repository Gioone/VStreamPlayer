using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VStreamPlayer.Commands
{
    public class PlayCommand
    {
        public static readonly RoutedUICommand Command;

        static PlayCommand()
        {
            Command = new RoutedUICommand("PlayCommand", "PlayCommand", typeof(PlayCommand));
            /*
            InputGestureCollection collection = new InputGestureCollection();
            KeyGesture gesture = new KeyGesture(Key.Space);
            collection.Add(gesture);

            Command.InputGestures.Add(gesture);
            */
        }

    }
}
