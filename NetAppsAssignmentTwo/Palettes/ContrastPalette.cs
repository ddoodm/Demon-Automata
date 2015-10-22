using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo.Palettes
{
    public class ContrastPalette : Palette
    {
        private const int
            MAX_LUM = 255,
            MAGIC_HALF = 2;

        public ContrastPalette(uint colorCount)
            : base(colorCount)
        {
            int i;
            for (i = 0; i < clut.Length / MAGIC_HALF; i++)
                clut[i] = Color.LightGray;
            for (; i < clut.Length; i++)
                clut[i] = Color.Black;

            ClutToBrushes();
        }
    }
}
