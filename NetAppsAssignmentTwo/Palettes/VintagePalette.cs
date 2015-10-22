using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo.Palettes
{
    public class VintagePalette : Palette
    {
        private const int
            RED_BASE = 225, GREEN_BASE = 240, BLUE_BASE = 180;

        private const float
            RED_CHROM = 50.0f, GREEN_CHROM = 200.0f, BLUE_CHROM = 180.0f;

        public VintagePalette(uint colorCount)
            : base(colorCount)
        {
            // Compute the color lookup table
            for (int i = 0; i < clut.Length; i++)
            {
                float luminance = (float)i / (float)clut.Length;

                clut[i] = Color.FromArgb(
                    RED_BASE - (int)(luminance * RED_CHROM),
                    GREEN_BASE - (int)(luminance * GREEN_CHROM),
                    BLUE_BASE - (int)(luminance * BLUE_CHROM));
            }

            ClutToBrushes();
        }
    }
}
