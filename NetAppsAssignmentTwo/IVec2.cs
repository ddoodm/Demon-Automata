using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo
{
    /// <summary>
    /// A discrete (integer) 2D vector
    /// </summary>
    class IVec2
    {
        public int x { get; private set; }
        public int y { get; private set; }

        public IVec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point asPoint
        {
            get { return new Point(x, y); }
        }
    }
}
