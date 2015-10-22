using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo
{
    /// <summary>
    /// A Cell (pixel) of the automata
    /// </summary>
    class Cell
    {
        /// <summary>
        /// The visual (draw) size of the cell
        /// </summary>
        public const int CELL_SIZE = 2;

        /// <summary>
        /// The coordinates of this cell in the grid
        /// </summary>
        public IVec2 position { get; private set; }

        /// <summary>
        /// The state of this cell
        /// </summary>
        public CellState state { get; private set; }
        private CellState tempState;

        /// <summary>
        /// The visual rectangle for this cell (used for drawing)
        /// </summary>
        public Rectangle visualRect { get; private set; }

        /// <summary>
        /// Defines the 'neighbor skipping rule' to be applied.
        /// </summary>
        /// <returns>True if this neighbor should be skipped, false otherwise.</returns>
        public delegate bool RuleDelegate(int relativeX, int relativeY);

        /// <summary>
        /// A reference to the parent cell grid
        /// </summary>
        private CellGrid grid;

        /// <summary>
        /// Instantiate a new Cell
        /// </summary>
        /// <param name="grid">A reference to the parent grid</param>
        /// <param name="position">The coordinates of this cell</param>
        public Cell(CellGrid grid, IVec2 position)
        {
            this.grid = grid;
            this.position = position;
            state = 0;

            InitGraphics();
        }

        /// <summary>
        /// Initialize the cell's graphics components
        /// </summary>
        private void InitGraphics()
        {
            Point visualPosition = new Point();
            visualPosition = position.asPoint;
            visualPosition.X *= CELL_SIZE;
            visualPosition.Y *= CELL_SIZE;
            visualRect = new Rectangle(
                visualPosition,
                new Size(CELL_SIZE, CELL_SIZE));
        }

        /// <summary>
        /// Compute the automata for this cell
        /// </summary>
        /// <param name="ruleAppliesFor">The 'neighbor skipping rule' to apply</param>
        public void ComputeNextState(RuleDelegate ruleAppliesFor)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Skip the middle cell (self)
                    if (x == 0 && y == 0)
                        continue;

                    // Apply the skipping rule
                    if (ruleAppliesFor(x, y))
                        continue;

                    // Obtain this cell's neighbor at offset (x,y) from the grid
                    Cell neighbor = grid.GetNeighborFor(this, x, y);

                    // If any of the cell?s four ... neighbours has
                    // the next state on from the cell
                    if (state.nextState == neighbor.state)
                    { tempState = state.nextState; return; }
                }
            }

            // No rules were met; return the current state
            tempState = this.state;
        }

        /// <summary>
        /// Skips diagonal neighbors
        /// </summary>
        /// <returns>True if the neighbor should be skipped</returns>
        public static bool OrthogonalRule(int relativeX, int relativeY)
        {
            return Utility.IntAbs(relativeX) == Utility.IntAbs(relativeY);
        }

        /// <summary>
        /// Skips orthogonal neighbors
        /// </summary>
        /// <returns>True if the neighbor should be skipped</returns>
        public static bool DiagonalRule(int relativeX, int relativeY)
        {
            // Do not calculate orthogonal neighbors
            return Utility.IntAbs(relativeX) != Utility.IntAbs(relativeY);
        }

        /// <summary>
        /// Skips cells on a corner
        /// </summary>
        /// <returns>True if the neighbor should be skipped</returns>
        public static bool CustomRule(int relativeX, int relativeY)
        {
            // Calculate cells for one corner
            return relativeX + relativeY > 0;
        }

        /// <summary>
        /// Update the current state of the cell to the temporary state
        /// </summary>
        public void ApplyTempState()
        {
            this.state = tempState;
        }

        /// <summary>
        /// Randomize the state of this cell
        /// </summary>
        /// <param name="r">A random number generator</param>
        public void Randomize(Random r)
        {
            state = r.Next(CellState.NUM_STATES);
        }
    }
}
