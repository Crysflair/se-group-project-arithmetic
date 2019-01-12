using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    public class Number
    {
        private readonly int Numerator;
        private readonly int Denominator;

        public Number(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public Number Add(Number a, Number b)
        {

        }

        public Number Sub(Number a, Number b)
        {

        }

        public Number Mul(Number a, Number b)
        {

        }

        public Number Div(Number a, Number b)
        {
            // TODO: Maybe we should check that b is not zero
        }

        public Number Pow(Number a, Number b)
        {
            // TODO: Maybe Number b must be a integar
        }

    }
}
