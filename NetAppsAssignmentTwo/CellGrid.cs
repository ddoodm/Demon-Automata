using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace NetAppsAssignmentTwo
{
    class CellGrid
    {
        public const int
            COLS = 240,
            ROWS = 320;

        private Cell[,] cells;
        private Rectangle[][] cellRectangles;

        public Palette palette { private get; set; }

        public CellGrid(Palette palette)
        {
            InitializeCells();
            InitializeGraphics();

            this.palette = palette;
        }

        private void GridLoop(Action<int,int> body)
        {
            for (int x = 0; x < COLS; x++)
                for (int y = 0; y < ROWS; y++)
                    body(x, y);
        }

        private void GridLoopParallel(Action<int, int> body)
        {
            Parallel.For(0, COLS, x =>
            {
                for (int y = 0; y < ROWS; y++)
                    body(x, y);
            });
        }

        private void InitializeCells()
        {
            // Create 2D cell array and initialize its elements
            cells = new Cell[COLS, ROWS];
            GridLoopParallel((x, y) =>
            {
                cells[x, y] = new Cell(this, new IVec2(x, y));
            });
        }

        private void InitializeGraphics()
        {
            cellRectangles = new Rectangle[CellState.NUM_STATES][];

            for (int s = 0; s < CellState.NUM_STATES; s++)
                cellRectangles[s] = new Rectangle[CellGrid.COLS * CellGrid.ROWS];
        }

        public void Randomize(int seed)
        {
            Random randGen = new Random(seed);

            GridLoop((x, y) =>
            {
                cells[x, y].Randomize(randGen);
            });
        }

        public void RunAutomata(AutomataRule rule)
        {
            // Compute the next state for each cell, do not apply yet
            GridLoopParallel((x, y) =>
            {
                cells[x, y].ComputeNextState(rule);
            });

            // Apply the temp. states after computation is complete
            GridLoopParallel((x, y) =>
            {
                cells[x, y].ApplyTempState();
            });
        }

        public Cell GetNeighborFor(Cell cell, IVec2 offset)
        {
            // Obtain the 'world wrapped' cell coordinates
            IVec2 wrappedPos = new IVec2(
                Utility.WrapModulo(cell.position.x + offset.x, CellGrid.COLS),
                Utility.WrapModulo(cell.position.y + offset.y, CellGrid.ROWS));

            // Obtain the cell
            return cells[wrappedPos.x, wrappedPos.y];
        }

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

        public void Draw(Graphics graphics)
        {
            Parallel.For(0, CellState.NUM_STATES, s =>
            {
                Array.Clear(cellRectangles[s], 0, cellRectangles[s].Length);
            });

            GridLoopParallel((x, y) =>
            {
                cellRectangles[cells[x, y].state][x + COLS * y]
                    = cells[x, y].visualRect;
            });

            for(int s = 0; s < CellState.NUM_STATES; s++)
            {
                Color cellColor = palette.StateToColor(s);
                Brush cellBrush = new SolidBrush(cellColor);
                graphics.FillRectangles(cellBrush, cellRectangles[s]);
            }
        }
    }
}
