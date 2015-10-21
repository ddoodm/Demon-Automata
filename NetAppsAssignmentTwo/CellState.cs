using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetAppsAssignmentTwo
{
    public class CellState
    {
        public const int NUM_STATES = 8;

        private int _numericState;
        public int numericState
        {
            get
            {
                return Utility.WrapModulo(_numericState, NUM_STATES);
            }
            set
            {
                _numericState = Utility.WrapModulo(value, NUM_STATES);
            }
        }

        public CellState(int numericValue = 0)
        {
            this.numericState = numericValue;
        }

        public CellState nextState
        {
            get { return this + 1; }
        }

        public CellState prevState
        {
            get { return this - 1; }
        }

        public static bool operator== (CellState lhs, CellState rhs)
        {
            return lhs.numericState == rhs.numericState;
        }

        public static bool operator!= (CellState lhs, CellState rhs)
        {
            return lhs.numericState != rhs.numericState;
        }

        public static implicit operator CellState(int rhs)
        {
            return new CellState(rhs);
        }

        public static implicit operator int(CellState rhs)
        {
            return rhs.numericState;
        }
    }
}
