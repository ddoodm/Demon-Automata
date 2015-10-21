using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo
{
    /// <summary>
    /// A discrete (integer) 2D vector
    /// </summary>
    struct IVec2
    {
        public int x, y;

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
