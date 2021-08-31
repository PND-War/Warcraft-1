using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Logic_Classes
{
    public class Group
    {
        public Point FocusedUnit;

        public Group()
        {
            FocusedUnit = new Point(-1, -1);
        }
    }
}
