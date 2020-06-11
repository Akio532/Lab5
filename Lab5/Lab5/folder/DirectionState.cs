using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using Bug;

namespace CockroachApp.folder
{
    class DirectionState
    {
        protected Dictionary<string, direction> Trends = new Dictionary<string, direction>()
        {
            {"Up", direction.Up },
            {"Down", direction.Down },
            {"Left", direction.Left },
            {"Right", direction.Right}
        };
        protected Bitmap image;
        protected const int step = 30;
    }
}
