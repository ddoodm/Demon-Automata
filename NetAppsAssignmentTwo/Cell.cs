using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo
{
    class Cell
    {
        public const int CELL_SIZE = 2;

        public IVec2 position { get; private set; }
        public CellState state { get; private set; }
        private CellState tempState;

        public Rectangle visualRect { get; private set; }

        public delegate bool RuleDelegate(IVec2 relativeCoords);

        private CellGrid grid;

        public Cell(CellGrid grid, IVec2 position)
        {
            this.grid = grid;
            this.position = position;
            state = 0;

            InitGraphics();
        }

        private void InitGraphics()
        {
            Point visualPosition = new Point();
            visualPosition.X = position.y * CELL_SIZE;
            visualPosition.Y = position.x * CELL_SIZE;
            visualRect = new Rectangle(
                visualPosition,
                new Size(CELL_SIZE, CELL_SIZE));
        }

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
                    if (this.nextState == neighbor.state)
                    { tempState = this.nextState; return; }
                }

            // No rules were met; return the current state
            tempState = this.state;
        }

        public static bool OrthogonalRule(IVec2 relativeCoords)
        {
            // Do not calculate diagonal neighbors
            return Math.Abs(relativeCoords.x) == Math.Abs(relativeCoords.y);
        }

        public static bool DiagonalRule(IVec2 relativeCoords)
        {
            // Do not calculate orthogonal neighbors
            return Math.Abs(relativeCoords.x) != Math.Abs(relativeCoords.y);
        }

        public void ApplyTempState()
        {
            this.state = tempState;
        }

        public CellState nextState
        {
            get { return state + 1; }
        }

        public void Randomize(Random r)
        {
            state = r.Next(CellState.NUM_STATES);
        }
    }
}
