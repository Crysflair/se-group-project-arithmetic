using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] symbol_set = { '+', '/', };
            string[] symbol_print = { "+", "÷" };
            QuestionGenerator generator = new QuestionGenerator(
                num_range_low: 2, num_range_high: 4,
                use_fraction: 0.5, MaxNodeCeiling: 3,
                symbol_set: symbol_set, symbol_print: symbol_print);
            generator.Generate(4, "file", "file2");
            Console.Read();

        }
        
        
    }
}
