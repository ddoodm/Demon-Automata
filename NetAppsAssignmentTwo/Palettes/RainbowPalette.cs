using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo.Palettes
{
    public class RainbowPalette : Palette
    {
        private Brush[] rainbowPal;

        public RainbowPalette()
        {
            rainbowPal = new Brush[]
            {
                Brushes.Red, Brushes.DarkViolet, Brushes.Blue,
                Brushes.LightBlue, Brushes.Navy, Brushes.Green,
                Brushes.Yellow, Brushes.Orange
            };
        }

        public override Brush StateToBrush(CellState state)
        {
            return rainbowPal[state % rainbowPal.Length];
        }
    }
}
