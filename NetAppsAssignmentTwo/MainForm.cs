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

using NetAppsAssignmentTwo.Palettes;

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

        /// <summary>
        /// The thread that executes the automata and rendering
        /// computations separate from the UI thread.
        /// </summary>
        private Thread computeThread;

        /// <summary>
        /// The grid of cells
        /// </summary>
        private CellGrid cellGrid;

        /// <summary>
        /// Initial constants
        /// </summary>
        private const int
            INITIAL_GENERATIONS = 100,
            INITIAL_SEED = 0;

        /// <summary>
        /// Used to convey generation status updates to the UI thread
        /// </summary>
        /// <param name="generation"></param>
        private delegate void UiStatusUpdateDelegate(int generation);

        public MainForm()
        {
            InitializeComponent();

            // Create and randomize the grid
            cellGrid = new CellGrid(Palette.MakePalette(0));
            cellGrid.Randomize(0);

            // Resize the form to fit the grid
            InitializeFormSize();

            // Set up the renderer and prepare an initial render
            InitializeGraphics();
            RenderImage();

            // Provide data sources and initial values to form controls
            InitializeControlValues();
        }

        /// <summary>
        /// Resize the form to fit the grid
        /// </summary>
        private void InitializeFormSize()
        {
            // Resize the panel to fit the grid
            panel_display.Size = new Size(
                CellGrid.COLS * Cell.CELL_SIZE,
                CellGrid.ROWS * Cell.CELL_SIZE);

            // Obtain sizes of form panels
            int displayHeight = panel_display.Height;
            int controlPanelHeight = panel_controls.Height;
            int statusStripHeight = statusStrip.Height;

            // Resize the form to fit all panels.
            // Anchors automatically position child components
            this.Size = new Size(
                panel_display.Width,
                displayHeight + controlPanelHeight + statusStripHeight);
        }

        /// <summary>
        /// Prepare the graphics context and the front and back buffers
        /// </summary>
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

        /// <summary>
        /// Draw the cell grid and update the front buffer
        /// </summary>
        private void RenderImage()
        {
            cellGrid.Draw(backGraphics);
            updateFrontBuffer();
        }

        /// <summary>
        /// Draw the back-buffer to the display
        /// </summary>
        private void updateFrontBuffer()
        {
            panelGraphics.DrawImageUnscaled(backBuffer, 0, 0);
        }

        /// <summary>
        /// Provide data sources and initial values to form controls
        /// </summary>
        private void InitializeControlValues()
        {
            // Disable form resizing
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MinimizeBox = MaximizeBox = false;

            // Initialize text boxes with initial values
            tb_gens.Text = INITIAL_GENERATIONS.ToString();
            tb_seed.Text = INITIAL_SEED.ToString();

            // Give Combo Boxes their data sets (enums)
            combo_rules.DataSource = Enum.GetValues(typeof(AutomataRule));
            combo_colours.DataSource = Enum.GetValues(typeof(PaletteName));

            // Compute and display the initial hash value
            DisplayHash();
        }

        private void panel_display_Paint(object sender, PaintEventArgs e)
        {
            // Draw back-buffer to the panel whenever the display is invalid,
            // but do not update the front buffer if it is being updated by the
            // compute thread.
            if(computeThread == null || !computeThread.IsAlive)
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

            ResetGrid(seed.Value);
        }

        /// <summary>
        /// Initialize the grid to random states using the given seed
        /// </summary>
        /// <param name="seed">The random number generator seed</param>
        private void ResetGrid(int seed)
        {
            cellGrid.Randomize(seed);
            RenderImage();

            // Reset the generation count and compute the initial hash
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

            // Parse the selected list item to a rule
            // (list contains only AutomataRule names)
            AutomataRule rule = AutomataRule.Orthogonal;
            Enum.TryParse<AutomataRule>(combo_rules.Text, out rule);

            // Initialize progress maximum
            progress_generations.Maximum = generations.Value;

            RunAutomata(rule, generations.Value);
        }

        /// <summary>
        /// Run the automata on the grid,
        /// applying the given rule for the
        /// specified number of generations.
        /// </summary>
        /// <param name="rule">The name of the rule to apply to the grid</param>
        /// <param name="generations">The number of times to execute the automata</param>
        private void RunAutomata(AutomataRule rule, int generations)
        {
            // Do not lock the UI thread
            computeThread = new Thread(() =>
            {
                // Disable form controls
                this.Invoke((Action)(() => panel_controls.Enabled = false));

                for (int gen = 1; gen <= generations; gen++)
                {
                    // Compute this generation and render
                    cellGrid.RunAutomata(rule);
                    RenderImage();

                    // Update the generation count UI
                    this.Invoke((UiStatusUpdateDelegate)UpdateGenerationUI, gen);
                }

                // Compute and display the final hash, and enable the controls
                this.Invoke((Action)DisplayHash);
                this.Invoke((Action)(() => panel_controls.Enabled = true));
            });

            computeThread.Start();
        }

        /// <summary>
        /// Update the progress bar and status label to show the
        /// progress through the simulation.
        /// </summary>
        /// <param name="generation">The current generation number</param>
        public void UpdateGenerationUI(int generation)
        {
            status_generation.Text = generation.ToString();
            progress_generations.Value = generation;
        }

        /// <summary>
        /// Compute the hash for the grid of cells,
        /// and display it on the status bar.
        /// </summary>
        public void DisplayHash()
        {
            uint hash = cellGrid.ComputeHash();
            status_hash.Text = hash.ToString();
        }

        /// <summary>
        /// Parses the Text value of a control to an integer.
        /// </summary>
        /// <typeparam name="ControlType">The type of the control</typeparam>
        /// <param name="control">The control</param>
        /// <returns>The text value of the control as an integer, null otherwise</returns>
        private int? ParseControlTextToInt<ControlType>(object control) where ControlType : Control
        {
            int newValue;
            if (int.TryParse(((ControlType)control).Text, out newValue))
                return newValue;
            return null;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If the compute thread is still running, stop it before closing
            if(computeThread != null)
                computeThread.Abort();
        }

        private void combo_colours_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Redraw the grid with the palette specified in the palette combo box
            PaletteName palName;
            if (Enum.TryParse<PaletteName>(combo_colours.Text, out palName))
            {
                cellGrid.palette = Palette.MakePalette(palName);
                RenderImage();
            }
        }
    }
}
