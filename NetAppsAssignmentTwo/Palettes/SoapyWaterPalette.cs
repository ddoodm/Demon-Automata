using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo.Palettes
{
    public class SoapyWaterPalette : Palette
    {
        private const float
            BRIGHTNESS = 200.0f,
            FALLOFF = 100.0f;
        private const int
            BASE_BRIGHTNESS = 200,
            SHIFT = 20;

        public SoapyWaterPalette(uint colorCount)
            : base(colorCount)
        {
            // Compute the color lookup table
            for (int i = 0; i < clut.Length; i++)
            {
                float luminance = (float)i / (float)clut.Length;
                int brightness = BASE_BRIGHTNESS;

                clut[i] = Color.FromArgb(
                    brightness - (int)(luminance * BRIGHTNESS),
                    brightness += SHIFT - (int)(luminance * (BRIGHTNESS - FALLOFF)),
                    brightness += SHIFT - (int)(luminance * (BRIGHTNESS - FALLOFF - FALLOFF)));
            }

            ClutToBrushes();
        }
    }
}
