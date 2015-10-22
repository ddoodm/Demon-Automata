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

        /// <summary>
        /// Computes the absolute value for integer 'num',
        /// and returns an unsigned integer
        /// </summary>
        /// <param name="num">The input number to compute</param>
        /// <returns>The absolute value of 'num'</returns>
        public static uint IntAbs(int num)
        {
            return (uint)(num > 0 ? num : checked(-num));
        }

        /// <summary>
        /// Determines whether a T is equal to any of the parameters.
        /// </summary>
        /// <typeparam name="T">The type of both sides of the comparison</typeparam>
        /// <param name="theComparitor">The object against which the comparison occurs</param>
        /// <param name="theComparisons">The objects to compare with</param>
        /// <returns>True if any of theComparisons matches theEnum</returns>
        public static bool EnumIsAnyOf<T>(T theComparitor, params T[] theComparisons)
            where T : IConvertible
        {
            foreach (T comp in theComparisons)
            {
                if (theComparitor.Equals(comp))
                    return true;
            }
            return false;
        }
    }
}
