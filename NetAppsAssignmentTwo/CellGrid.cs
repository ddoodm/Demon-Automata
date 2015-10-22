using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

using NetAppsAssignmentTwo.Palettes;

namespace NetAppsAssignmentTwo
{
    /// <summary>
    /// Names for automata skipping rules
    /// </summary>
    public enum AutomataRule
    {
        Orthogonal, Diagonal, Alternating, CustomRule
    }

    /// <summary>
    /// A grid of Cells
    /// </summary>
    class CellGrid
    {
        /// <summary>
        /// The constant initial size of the grid
        /// (the number of cells in the grid).
        /// </summary>
        public const int
            ROWS = 240,
            COLS = 320;

        /// <summary>
        /// The 2D array grid of cells
        /// </summary>
        private Cell[,] cells;

        /// <summary>
        /// A list of rectangle arrays used for more efficient drawing.
        /// Array indexes correspond to cell states (0 - 7 normally).
        /// Each list contains the cells in this generation that have
        /// a state that is equal to the list's index.
        /// 
        /// Graphics.FillRectangles() is given each list as an array
        /// for efficient batched drawing.
        /// </summary>
        private List<Rectangle>[] cellRectangles;

        /// <summary>
        /// The colour palette (lookup table) to use when drawing each cell.
        /// </summary>
        public Palette palette { private get; set; }

        public CellGrid(Palette palette)
        {
            InitializeCells();
            InitializeGraphics();

            this.palette = palette;
        }

        /// <summary>
        /// Defines a loop that iterates over every cell linearly.
        /// </summary>
        /// <param name="body">The action to execute for each cell</param>
        private void GridLoop(Action<int,int> body)
        {
            for (int y = 0; y < ROWS; y++)
            {
                for (int x = 0; x < COLS; x++)
                    body(x, y);
            }
        }

        /// <summary>
        /// Defines a loop that iterates over every cell
        /// in parallel (threaded). Executes each row
        /// concurrently when possible.
        /// </summary>
        /// <param name="body">The action to execute for each cell</param>
        private void GridLoopParallel(Action<int, int> body)
        {
            Parallel.For(0, ROWS, y =>
            {
                for (int x = 0; x < COLS; x++)
                    body(x, y);
            });
        }

        /// <summary>
        /// Initialize the cell grid array
        /// </summary>
        private void InitializeCells()
        {
            // Create 2D cell array and initialize its elements
            cells = new Cell[COLS, ROWS];
            GridLoopParallel((x, y) =>
            {
                cells[x, y] = new Cell(this, new IVec2(x, y));
            });
        }

        /// <summary>
        /// Initialize the graphics objects (rectangles).
        /// </summary>
        private void InitializeGraphics()
        {
            cellRectangles = new List<Rectangle>[CellState.NUM_STATES];

            for (int s = 0; s < CellState.NUM_STATES; s++)
                cellRectangles[s] = new List<Rectangle>();
        }

        /// <summary>
        /// Randomize all cells
        /// </summary>
        /// <param name="seed">The random number generator seed</param>
        public void Randomize(int seed)
        {
            Random randGen = new Random(seed);

            GridLoop((x, y) =>
            {
                cells[x, y].Randomize(randGen);
            });
        }

        /// <summary>
        /// Run one generation of the automata on the grid.
        /// </summary>
        /// <param name="rule">The 'neighbor skipping rule' to apply</param>
        public void RunAutomata(AutomataRule rule)
        {
            // Empty multi-cast delegate
            Action automataAction = () => { };

            // Add the OrthogonalRule automata for any rule that requires it
            if(Utility.EnumIsAnyOf<AutomataRule>(rule,
                AutomataRule.Alternating, AutomataRule.Orthogonal, AutomataRule.CustomRule))
                automataAction += () => RunAutomata(Cell.OrthogonalRule);

            // Add the DiagonalRule automata for any rule that requires it
            if (Utility.EnumIsAnyOf<AutomataRule>(rule,
                AutomataRule.Alternating, AutomataRule.Diagonal))
                automataAction += () => RunAutomata(Cell.DiagonalRule);

            // Add the CustomRule automata if required
            if(rule == AutomataRule.CustomRule)
                automataAction += () => RunAutomata(Cell.CustomRule);

            // Run
            automataAction();
        }

        /// <summary>
        /// Execute the automata on every cell
        /// </summary>
        /// <param name="rule">The rule function</param>
        private void RunAutomata(Cell.RuleDelegate rule)
        {
            // Compute the next state for each cell concurrently,
            // do not apply yet
            GridLoopParallel((x, y) =>
            {
                cells[x, y].ComputeNextState(rule);
            });

            // Apply the temp. states after computation is complete
            GridLoop((x, y) =>
            {
                cells[x, y].ApplyTempState();
            });
        }

        /// <summary>
        /// Gets the cell that is a neighbor to the specified cell
        /// </summary>
        /// <param name="cell">The cell for which to find a neighbor</param>
        /// <returns>The neighbor at the specified offset from the cell</returns>
        public Cell GetNeighborFor(Cell cell, int offsetX, int offsetY)
        {
            // Obtain the 'world wrapped' cell coordinates
            int wrappedX = Utility.WrapModulo(cell.position.x + offsetX, CellGrid.COLS);
            int wrappedY = Utility.WrapModulo(cell.position.y + offsetY, CellGrid.ROWS);

            // Obtain the cell
            return cells[wrappedX, wrappedY];
        }

        /// <summary>
        /// Computes the hash value for the grid
        /// </summary>
        /// <returns>The grid's hash value</returns>
        public uint ComputeHash()
        {
            int state, hash = 0;
            GridLoop((col, row) =>
            {
                state = cells[col, row].state;
                hash ^= ((row * col + 1) * (state + 1));
            });
            return (uint)hash; 
        }

        /// <summary>
        /// Draws the grid to the graphics device
        /// </summary>
        /// <param name="graphics">The graphics for drawing</param>
        public void Draw(Graphics graphics)
        {
            // Clear the old set of states
            for(int s = 0; s < CellState.NUM_STATES; s++)
                cellRectangles[s].Clear();

            // Concurrently determine which batch each cell should
            // be drawn in.
            GridLoopParallel((x, y) =>
            {
                Cell currentCell = cells[x, y];
                int stateId = currentCell.state;
                lock (cellRectangles) { cellRectangles[stateId].Add(currentCell.visualRect); }
            });

            // Concurrently convert each list to an array
            Rectangle[][] rectArray = new Rectangle[CellState.NUM_STATES][];
            Parallel.For(0, CellState.NUM_STATES, s =>
            {
                rectArray[s] = cellRectangles[s].ToArray();
            });

            // Linearly draw each rectangle brush with the
            // appropriate colour from the palette
            for(int s = 0; s < CellState.NUM_STATES; s++)
            {
                if (rectArray[s].Length < 1)
                    continue;

                Brush cellBrush = palette.StateToBrush(s);
                graphics.FillRectangles(cellBrush, rectArray[s]);
            }
        }
    }
}
