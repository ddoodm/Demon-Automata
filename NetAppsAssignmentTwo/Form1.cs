using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetAppsAssignmentTwo
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The buffered image (before being displayed)
        /// </summary>
        private Bitmap backBuffer = null;

        /// <summary>
        /// The draw and display graphics objects (respectively)
        /// for double-buffered rendering.
        /// </summary>
        private Graphics backGraphics = null, panelGraphics = null;

        private Thread computeThread;
        private delegate void UiStatusUpdateDelegate(int generation);

        private CellGrid cellGrid;

        private int generations;

        private const PaletteName INITIAL_PALETTE = 0;

        private const int
            INITIAL_GENERATIONS = 100,
            INITIAL_SEED = 0;

        public MainForm()
        {
            InitializeComponent();

            cellGrid = new CellGrid(Palette.MakePalette(INITIAL_PALETTE));
            cellGrid.Randomize(0);

            InitializeFormSize();

            InitializeGraphics();
            renderImage();

            InitializeControlValues();
        }

        private void InitializeFormSize()
        {
            panel_display.Size = new Size(
                CellGrid.ROWS * Cell.CELL_SIZE,
                CellGrid.COLS * Cell.CELL_SIZE);

            int displayHeight = panel_display.Height;
            int controlPanelHeight = panel_controls.Height;
            int statusStripHeight = statusStrip.Height;

            this.Size = new Size(
                panel_display.Width,
                displayHeight + controlPanelHeight + statusStripHeight);
        }

        private void InitializeGraphics()
        {
            // Dispose old graphics resources
            if (panelGraphics != null) panelGraphics.Dispose();
            if (backGraphics != null) backGraphics.Dispose();
            if (backBuffer != null) backBuffer.Dispose();

            // Allow access to the panel's renderer
            panelGraphics = panel_display.CreateGraphics();

            // Initialize the back-buffer for drawing
            backBuffer = new Bitmap(panel_display.Width, panel_display.Height);
            backGraphics = Graphics.FromImage(backBuffer);
        }

        private void renderImage()
        {
            cellGrid.Draw(backGraphics);
            updateFrontBuffer();
        }

        private void updateFrontBuffer()
        {
            panelGraphics.DrawImageUnscaled(backBuffer, 0, 0);
        }

        private void InitializeControlValues()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimizeBox = MaximizeBox = false;

            tb_gens.Text = INITIAL_GENERATIONS.ToString();
            tb_seed.Text = INITIAL_SEED.ToString();

            // Give Combo Boxes their data sets
            combo_rules.DataSource = Enum.GetValues(typeof(AutomataRule));
            combo_colours.DataSource = Enum.GetValues(typeof(PaletteName));

            // Compute and display the initial hash value
            DisplayHash();
        }

        private void panel_display_Paint(object sender, PaintEventArgs e)
        {
            // Draw back-buffer to the panel
            // whenever the panel is invalid
            updateFrontBuffer();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            // Obtain the random number generator's seed from the form's TextBox
            int? seed = ParseControlTextToInt<TextBox>(tb_seed);
            if(!seed.HasValue)
            {
                MessageBox.Show("Failed to reset the grid.\nThe seed is invalid. Please supply an integer.");
                return;
            }

            cellGrid.Randomize(seed.Value);
            renderImage();

            UpdateGenerationUI(0);
            DisplayHash();
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            // Obtain the requested number of iterations from the form's TextBox
            int? generations = ParseControlTextToInt<TextBox>(tb_gens);
            if (!generations.HasValue || generations.Value <= 0)
            {
                MessageBox.Show("Failed to start Demon.\n\"Gens\" must be an integer greater than 0.");
                return;
            }
            this.generations = generations.Value;

            AutomataRule rule = AutomataRule.Orthogonal;
            Enum.TryParse<AutomataRule>(combo_rules.Text, out rule);

            computeThread = new Thread(new ThreadStart(delegate
            {
                this.Invoke((Action)delegate { panel_controls.Enabled = false; });

                lock (cellGrid)
                {
                    for (int gen = 1; gen <= generations; gen++)
                    {
                        cellGrid.RunAutomata(rule);
                        renderImage();

                        this.Invoke((UiStatusUpdateDelegate)UpdateGenerationUI, gen);
                    }
                }

                this.Invoke((Action)DisplayHash);
                this.Invoke((Action)delegate { panel_controls.Enabled = true; });
            }));

            computeThread.Start();
        }

        public void UpdateGenerationUI(int generation)
        {
            status_generation.Text = generation.ToString();

            progress_generations.Maximum = generations;
            progress_generations.Value = generation;
        }

        public void DisplayHash()
        {
            uint hash = cellGrid.ComputeHash();
            status_hash.Text = hash.ToString();
        }

        private int? ParseControlTextToInt<ControlType>(object sender) where ControlType : Control
        {
            int newValue;
            if (int.TryParse(((ControlType)sender).Text, out newValue))
                return newValue;
            return null;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(computeThread != null)
                computeThread.Abort();
        }

        private void combo_colours_SelectionChangeCommitted(object sender, EventArgs e)
        {
            PaletteName palName;
            if (Enum.TryParse<PaletteName>(combo_colours.Text, out palName))
            {
                cellGrid.palette = Palette.MakePalette(palName);
                renderImage();
            }
        }
    }
}
