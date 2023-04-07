using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VStreamPlayer.Core
{
    public class PlaySpeed
    {
        private double _speed = 1.0;

        public double Speed
        {
            get { return _speed; }
            set
            {
                ChangeSpeed(value);
            }
        }

        private void ChangeSpeed(double value)
        {
            double tmp = _speed;
            tmp += value;

            if (tmp is >= 5.0 or <= 0.25)
            {
                return;
            }

            _speed = tmp;
        }
    }
}
