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
            char[] symbol_set = { '+','-','*','/','^'};
            string[] symbol_print = { "+", "-", "*", "/", "^" };
            QuestionGenerator generator = new QuestionGenerator(
                num_range_low: -100000, num_range_high: 100000,
                use_fraction: 0.5, MaxNodeCeiling: 10,
                symbol_set: symbol_set, symbol_print: symbol_print);
            generator.Generate(1000);

            Tuple<List<string>,List<string>> QA_pairs = generator.Get_QA_pairs();
            var len = QA_pairs.Item1.Count();
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine($"{QA_pairs.Item1[i]} = {QA_pairs.Item2[i]}");
            }
            Console.Read();
            
        }   
    }
}
