using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo.Palettes
{
    public class HighTechPalette : Palette
    {
        private const float BRIGHTNESS = 200.0f;
        private const int BLUE_BASE = 55;

        public HighTechPalette(uint colorCount)
            : base(colorCount)
        {
            // Compute the color lookup table
            for (int i = 0; i < clut.Length; i++)
            {
                float luminance = (float)i / (float)clut.Length * BRIGHTNESS;

                clut[i] = Color.FromArgb(0, (int)(luminance), BLUE_BASE);
            }

            ClutToBrushes();
        }
    }
}
