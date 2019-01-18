using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arithmetic
{
    public class Standardizer
    {
        static Regex neg_digi = new Regex(@"^-(?<num1>\d*)\.(?<num2>\d*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex posi0_digi = new Regex(@"^(?<num1>\d*)\.(?<num2>\d*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex neg_int = new Regex(@"^-(?<num>\d*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex posi0_int = new Regex(@"^(?<num>\d*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex neg_frac = new Regex(@"^-(?<num1>\d*)/(?<num2>\d*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static Regex posi0_frac = new Regex(@"^(?<num1>\d*)/(?<num2>\d*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        static Number neg_one_node = new Number(-1, 1);
        static Number pos_one_node = new Number(1, 1);

        // facility
        public static string deci_standardize(string left_string, string right_string, bool is_neg)
        {
            Number left = new Number(int.Parse(left_string), 1);
            Number right = new Number(int.Parse(right_string), Number.IntPow(10, right_string.Length));
            Number if_neg_node;
            if (is_neg)
                if_neg_node = neg_one_node;
            else
                if_neg_node = pos_one_node;

            // construct expression
            Operation result_abs = new Operation(
                new VariableReference("left"), '+', new VariableReference("right"), 1
                );
            Operation result = new Operation(
                result_abs, '*', new VariableReference("if_neg_node"), 2
                );
            Dictionary<string, object> variable_to_number = new Dictionary<string, object> {
                    {"left", left },
                    {"right", right },
                    {"if_neg_node", if_neg_node}
                };

            return result.Evaluate(variable_to_number).ToString();
        }

        // CORE CODE
        public static string Standardize_Number(string input)
        {
            //// description:
            //
            //1. accepted format rule
            //  - space and comma can exist everywhere
            //  - no parenthesis !
            //  - one negative sign or no sign, at the beginning of the string.
            //  - fractions unreduced are supported
            //  - if answer can be converted to decimal, decimal equivalent form supported.
            //
            //2. convert to (m/n) or m if convertable
            //  else return "ERROR"
            //
            //3. examples
            //  "<-> <integar> </> <integar>"
            //  "<-> <integar> <.> <integar>"
            //  "<-> <integar> "
            //  "<integar> </> <integar>"
            //  "<integar> <.> <integar>"
            //  "<integar> "
            ////
            

            
            input = input.Replace(" ", "");
            input = input.Replace(",", "");
            MatchCollection matches = null;

            // 分数
            matches = neg_frac.Matches(input);
            if (matches.Count == 1) // Hit!
            {
                var match = matches[0];
                try //Try Parse
                {
                    int num1 = int.Parse(match.Groups["num1"].Value);
                    int num2 = int.Parse(match.Groups["num2"].Value);
                    return new Number(-num1, num2).ToString();
                }
                catch (Exception) {; }
                
            }

            matches = posi0_frac.Matches(input);
            if (matches.Count == 1) // Hit!
            {
                var match = matches[0];
                try
                {
                    int num1 = int.Parse(match.Groups["num1"].Value);
                    int num2 = int.Parse(match.Groups["num2"].Value);
                    return new Number(num1, num2).ToString();
                }
                catch (Exception) {; }

            }

            // 整数
            matches = neg_int.Matches(input);
            if (matches.Count == 1) // Hit!
            {
                var match = matches[0];
                try
                {
                    int num1 = int.Parse(match.Groups["num"].Value);
                    return new Number(-num1, 1).ToString();
                }
                catch (Exception) {; }

            }

            matches = posi0_int.Matches(input);
            if (matches.Count == 1) // Hit!
            {
                var match = matches[0];
                try
                {
                    int num1 = int.Parse(match.Groups["num"].Value);
                    return new Number(num1, 1).ToString();
                }
                catch (Exception) {; }

            }

            // 小数
            matches = neg_digi.Matches(input);
            if (matches.Count == 1) // Hit!
            {
                var match = matches[0];
                try
                {
                    string left_string = match.Groups["num1"].Value;
                    string right_string = match.Groups["num2"].Value;
                    return deci_standardize(left_string, right_string, is_neg: true);
                }
                catch (Exception) {; }

            }
            matches = posi0_digi.Matches(input);
            if (matches.Count == 1) // Hit!
            {
                var match = matches[0];
                try
                {
                    string left_string = match.Groups["num1"].Value;
                    string right_string = match.Groups["num2"].Value;
                    return deci_standardize(left_string, right_string, is_neg: false);
                }
                catch (Exception) {; }
            }

            return "ERROR";
        }
    }
}
