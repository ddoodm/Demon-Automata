using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetAppsAssignmentTwo
{
    class Utility
    {
        /// <summary>
        /// Unsigned integer-style modulo operation.
        /// Negative values are 'wrapped-around' to the value of the denominator
        /// </summary>
        /// <param name="numerator">Number (LHS)</param>
        /// <param name="denominator">Devisor (RHS)</param>
        /// <returns>The wrapped-around modulo</returns>
        public static int WrapModulo(int numerator, int denominator)
        {
            int remainder = numerator % denominator;
            return remainder < 0 ? remainder + denominator : remainder;
        }
    }
}
