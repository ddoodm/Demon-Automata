using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetAppsAssignmentTwo
{
    /// <summary>
    /// Describes the current state of a cell.
    /// Automatically wraps from 0 - CellState.NUM_STATES.
    /// </summary>
    public class CellState
    {
        /// <summary>
        /// The maximum state of any cell before it is wrapped to 0
        /// </summary>
        public const int NUM_STATES = 8;

        /// <summary>
        /// The numeric state of the cell
        /// </summary>
        private int _numericState;
        public int numericState
        {
            get { return _numericState; }
            set
            {
                // Automatically wrap the state value whenever it
                // is mutated
                _numericState = Utility.WrapModulo(value, NUM_STATES);
            }
        }

        /// <summary>
        /// Create a CellState
        /// </summary>
        /// <param name="numericValue">The initial state (not required)</param>
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

        public override bool Equals(object obj)
        {
            if (obj is CellState)
                return ((CellState)obj).numericState == this.numericState;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
