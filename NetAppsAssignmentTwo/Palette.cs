using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo.Palettes
{
    /// <summary>
    /// Names for the available colour palettes
    /// </summary>
    public enum PaletteName
    {
        HighTech, Vintage, SoapyWater, Rainbow, Contrast, Random
    }

    public abstract class Palette
    {
        /// <summary>
        /// The colour lookup table that this palette defines
        /// </summary>
        protected Color[] clut;

        /// <summary>
        /// The brushes that correspond to the colour lookup table
        /// </summary>
        protected Brush[] brushes;

        protected Palette () { }

        protected Palette(uint colorCount)
        {
            clut = new Color[colorCount];
            brushes = new Brush[colorCount];
        }

        /// <summary>
        /// The Palette Factory
        /// Creates a Palette given a Palette Name
        /// </summary>
        /// <param name="name">The name (enum) of the palette to create</param>
        /// <returns>A new palette of the type specified by 'name'</returns>
        public static Palette MakePalette(
            PaletteName name,
            uint colorCount = CellState.NUM_STATES)
        {
            switch(name)
            {
                default:
                case PaletteName.HighTech: return new HighTechPalette(colorCount);
                case PaletteName.SoapyWater: return new SoapyWaterPalette(colorCount);
                case PaletteName.Vintage: return new VintagePalette(colorCount);
                case PaletteName.Rainbow: return new RainbowPalette();
                case PaletteName.Contrast: return new ContrastPalette(colorCount);
                case PaletteName.Random: return new RandomPalette(colorCount);
            }
        }

        /// <summary>
        /// Generate the array of Brush from the CLUT
        /// </summary>
        protected void ClutToBrushes()
        {
            for(int i = 0; i < clut.Length; i++)
                brushes[i] = new SolidBrush(clut[i]);
        }

        /// <summary>
        /// Convert a state number to a Color
        /// </summary>
        public virtual Color StateToColor(CellState state)
        {
            return clut[state];
        }

        /// <summary>
        /// Convert a state number to a Brush
        /// </summary>
        public virtual Brush StateToBrush(CellState state)
        {
            return brushes[state];
        }
    }
}
