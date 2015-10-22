using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="relativeCoords">The 2D coordinates of this cell in the grid</param>
        /// <returns>True if this neighbor should be skipped, false otherwise.</returns>
        public delegate bool RuleDelegate(IVec2 relativeCoords);

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
            IVec2 offset;

            for (offset.x = -1; offset.x <= 1; offset.x++)
                for (offset.y = -1; offset.y <= 1; offset.y++)
                {
                    // Skip the middle cell (self)
                    if (offset.x == 0 && offset.y == 0)
                        continue;

                    // Apply the skipping rule
                    if (ruleAppliesFor(offset))
                        continue;

                    // Obtain this cell's neighbor at offset (x,y) from the grid
                    Cell neighbor = grid.GetNeighborFor(this, offset);

                    // If any of the cell’s four ... neighbours has
                    // the next state on from the cell
                    if (state.nextState == neighbor.state)
                    { tempState = state.nextState; return; }
                }

            // No rules were met; return the current state
            tempState = this.state;
        }

        /// <summary>
        /// Skips diagonal neighbors
        /// </summary>
        /// <param name="relativeCoords">The relative coordinates of the neighbor</param>
        /// <returns>True if the neighbor should be skipped</returns>
        public static bool OrthogonalRule(IVec2 relativeCoords)
        {
            return Utility.IntAbs(relativeCoords.x) == Utility.IntAbs(relativeCoords.y);
        }

        /// <summary>
        /// Skips orthogonal neighbors
        /// </summary>
        /// <param name="relativeCoords">The relative coordinates of the neighbor</param>
        /// <returns>True if the neighbor should be skipped</returns>
        public static bool DiagonalRule(IVec2 relativeCoords)
        {
            // Do not calculate orthogonal neighbors
            return Utility.IntAbs(relativeCoords.x) != Utility.IntAbs(relativeCoords.y);
        }

        /// <summary>
        /// Skips cells on a corner
        /// </summary>
        /// <param name="relativeCoords">The relative coordinates of the neighbor</param>
        /// <returns>True if the neighbor should be skipped</returns>
        public static bool CustomRule(IVec2 relativeCoords)
        {
            // Calculate cells for one corner
            return relativeCoords.x + relativeCoords.y > 0;
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
