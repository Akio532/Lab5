using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Bug.State
{
    interface IDirection
    {
        void Step(ref int X, ref int Y);
        IDirection ChangeTrend(string command);
        direction Trend { get; }

    }
    
}
