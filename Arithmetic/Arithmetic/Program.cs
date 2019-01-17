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
            char[] symbol_set = { '+'};
            string[] symbol_print = { "+"};
            QuestionGenerator generator = new QuestionGenerator(
                num_range_low: 2, num_range_high: 4,
                use_fraction: 0.5, MaxNodeCeiling: 2,
                symbol_set: symbol_set, symbol_print: symbol_print);
            generator.Generate(4);
            generator.Save_to_file(question_path: "./questions.txt", answer_path: "./answers.txt");
            Console.Read();
        }   
    }
}
