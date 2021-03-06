﻿using Bug.State;
using CockroachApp.State;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace Bug
{
    public class Cockroach
    {
        public Bitmap image;
        const int step = 30;
        int x;
        int y;
        IDirection direction;
        public Cockroach(Bitmap _image)
        {
            image = _image;
            direction = new DirectionUp(image);
            
        }
        
        public int X
        {
            get => x;
            set => x = value; 
        }
        public int Y 
        { 
            get => y; 
            set => y = value;
        }
        public Bitmap Image 
        { 
            get => image;
            set => image = value; 
        }

        public void newcoord(int dx, int dy)
        {
            x = dx;
            y = dy;
        }
        public void Step()
        {
            direction.Step(ref x, ref y);
        }

        //Изменение направления, параметр – первая буква направления
        public void ChangeTrend(string s)
        {
            direction = direction.ChangeTrend(s);
        }

      
    }
}
