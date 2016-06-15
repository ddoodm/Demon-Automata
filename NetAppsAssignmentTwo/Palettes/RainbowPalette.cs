using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo.Palettes
{
    public class RainbowPalette : Palette
    {
        private Color[] rainbowCol;
        private Brush[] rainbowBrush;

        public RainbowPalette()
        {
            rainbowCol = new Color[]
            {
                Color.Red, Color.DarkViolet, Color.Blue,
                Color.LightBlue, Color.Navy, Color.Green,
                Color.Yellow, Color.Orange
            };

            rainbowBrush = new Brush[rainbowCol.Length];
            for (int i = 0; i < rainbowBrush.Length; i++)
                rainbowBrush[i] = new SolidBrush(rainbowCol[i]);
        }

        public override Color StateToColor(CellState state)
        {
            return rainbowCol[state % rainbowCol.Length];
        }

        public override Brush StateToBrush(CellState state)
        {
            return rainbowBrush[state % rainbowBrush.Length];
        }
    }
}
