using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo.Palettes
{
    public class RandomPalette : Palette
    {
        private const int MAX_LUM = 255;
        private Random random = new Random();

        public RandomPalette(uint colorCount)
            : base(colorCount)
        {
            for (int i = 0; i < clut.Length; i++)
                clut[i] = Color.FromArgb(
                    random.Next() % MAX_LUM,
                    random.Next() % MAX_LUM,
                    random.Next() % MAX_LUM);

            ClutToBrushes();
        }
    }
}
