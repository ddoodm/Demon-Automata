using System;
using System.Collections.Generic;
using System.Linq;
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
        public static Palette MakePalette(PaletteName name, uint colorCount = CellState.NUM_STATES)
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

    public class HighTechPalette : Palette
    {
        private const float BRIGHTNESS = 200.0f;
        private const int BLUE_BASE = 55;

        public HighTechPalette(uint colorCount)
            : base(colorCount)
        {
            // Compute the color lookup table
            for(int i = 0; i < clut.Length; i++)
            {
                float luminance = (float)i / (float)clut.Length * BRIGHTNESS;

                clut[i] = Color.FromArgb(0, (int)(luminance), BLUE_BASE);
            }

            ClutToBrushes();
        }
    }

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

    public class RainbowPalette : Palette
    {
        private Brush[] rainbowPal;

        public RainbowPalette ()
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

    public class ContrastPalette : Palette
    {
        private const int MAX_LUM = 255;

        public ContrastPalette(uint colorCount)
            : base(colorCount)
        {
            int i;
            for (i = 0; i < clut.Length / 2; i++)
                clut[i] = Color.LightGray;
            for (; i < clut.Length; i++)
                clut[i] = Color.Black;

            ClutToBrushes();
        }
    }

    public class RandomPalette : Palette
    {
        private const int MAX_LUM = 255;
        private Random random = new Random();

        public RandomPalette(uint colorCount) : base(colorCount)
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
