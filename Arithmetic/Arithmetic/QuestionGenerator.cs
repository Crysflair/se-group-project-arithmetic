using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    public class QuestionGenerator
    {
        // Description:
        // 1. this class recieve specifications for question generation. This can be set only once.
        // 2. can generate differenct cnt of questions for multiple time.
        // 3. those questions are save to file in an WRITE manner, with a surplus blank line.
        // 4. if the user want to regenerate everything, delete those files via Question provider.

        private List<string> Expression_in_symbol;
        private List<string> Expression_in_number;
        private List<string> Ans;
        private static readonly string[] Default_Ref_to_Number_keys = { "a", "b", "c", "d", "e",
                                          "f", "g", "h", "i", "j",
                                          "k", "l", "m", "n", "o",
                                          "p", "q", "r", "s", "t",
                                          "u", "v", "w", "x", "y", "z" };

        private static Random rnd = new Random();

        private int num_range_low;
        private int num_range_high;
        private double use_fraction;
        private int MaxNodeCeiling;
        private char[] symbol_set;
        private string[] symbol_print;

        
        //// Constructor: check and save arguments
        public QuestionGenerator(
            int num_range_low, int num_range_high,        // 使用整数的数字范围 []
            double use_fraction,                           // 使用(真)分数的比例
            int MaxNodeCeiling,                           // 最多使用多少运算符(不多于10)
            char[] symbol_set,                            // 使用运算符种类, 如 [+, -, *, /, ^]
            string[] symbol_print                         // 使用运算符的显示方法, 如 [＋, -, ×, ÷, **]
            )
        {
            // argument checking
            if (num_range_high < num_range_low ||
                use_fraction > 1 || use_fraction < 0 ||
                MaxNodeCeiling > 10 ||
                symbol_print.Length != symbol_set.Length
                ) throw new ArgumentException("illegal argument for Question Generator!");
            // save parameters
            this.num_range_high = num_range_high;
            this.num_range_low = num_range_low;
            this.use_fraction = use_fraction;
            this.MaxNodeCeiling = MaxNodeCeiling;
            this.symbol_set = symbol_set;
            this.symbol_print = symbol_print;
        }

        //// CORE CODE. This can be generated for multiple time!
        public void Generate(int generate_cnt, int reuse_maxcnt = 5)            
        {
            // initiate(refresh)
            Expression_in_number = new List<string>();
            Expression_in_symbol = new List<string>();
            Ans = new List<string>();

            // start generate
            bool need_new_expression = true;  // 是否需要生成新的表达式符号
            TreeGenerator generator = null;   // 当前表达式生成器
            Expression expression = null;     // 当前表达式
            string expression_in_symbol = null;      // 当前表达式符号字符串

            while (generate_cnt > 0)
            {
                if (need_new_expression)      // 生成新的表达式符号
                {
                    // generate a new expression
                    generator = new TreeGenerator(symbol_set);
                    expression = generator.Generate(MaxNodeCeiling);
                    expression_in_symbol = Tree_to_Symbol_string(expression);

                    // check if duplicated
                    if (Expression_in_symbol.Contains(expression_in_symbol))
                        continue;
                }

                // if not duplicate, save this expression
                Expression_in_symbol.Add(expression_in_symbol);

                // 在当前符号表达式基础上生成多个数字表达式
                // then start generation !
                var SBpairs = generator.get_SBpairs();
                var Int_Node = generator.get_IntNodes();
                int symbol_to_number_failcnt = 0;
                int symbol_to_number_succeedcnt = 0;

                while (generate_cnt > 0)
                {
                    // 为当前表达式符号生成一种合法的赋值
                    Dictionary<string, object> Ref_to_Number = Assign_Ref_Value(num_range_low, num_range_high, use_fraction, SBpairs, Int_Node);
                    string expression_in_number = Fill_number(expression_in_symbol, Ref_to_Number);

                    if (Expression_in_number.Contains(expression_in_number))
                    {
                        symbol_to_number_failcnt += 1;
                        if (symbol_to_number_failcnt > 5) //如果失败太多次, 退出循环并请求生成新的字符串符号
                        {
                            need_new_expression = true;
                            break;
                        }
                        else continue;  //生成新的赋值
                    }
                    else // if this expression in number is brand new 
                    {
                        // insert if evaluable and not too difficult
                        Number ans;
                        try
                        {
                            ans = expression.Evaluate(Ref_to_Number);
                        }
                        catch(DivideByZeroException e)
                        {
                            continue;   //生成新的赋值
                        }
                         
                        if (ans.Denominator < 200 && Math.Abs(ans.Numerator) < 1000)
                        {
                            Expression_in_number.Add(expression_in_number);
                            Ans.Add(ans.ToString());
                            generate_cnt--;

                            symbol_to_number_succeedcnt++;
                            if (symbol_to_number_succeedcnt >= reuse_maxcnt)    // if use a patern too many times, change.
                            {
                                need_new_expression = true;
                                break;
                            }
                        }
                        else
                            continue;   //生成新的赋值
                    }
                }
            }
        }
        
        // facility: 使用字符串替换方法填充数字值和表达式自定义符号
        public string Fill_number(string expression_in_symbol, Dictionary<string, object> Ref_to_Number)
        {
            foreach (KeyValuePair<string, object> entry in Ref_to_Number)
            {
                // do something with entry.Value or entry.Key
                expression_in_symbol = expression_in_symbol.Replace(entry.Key, (entry.Value as Number).ToString());
            }
            for(int i=0; i < symbol_print.Length; i++ )
            {
                expression_in_symbol = expression_in_symbol.Replace(" " + symbol_set[i].ToString() + " ", " " + symbol_print[i] + " ");
            }
            return expression_in_symbol;
        }

        // facility: 生成合法赋值
        private static Dictionary<string, object> Assign_Ref_Value(
            int num_range_low, int num_range_high, double use_fraction,
            List<Tuple<string, string>> SBpairs, List<string> Int_Node)
        {
            // new an empty dictionary
            Dictionary<string, object> Ref_to_Number = new Dictionary<string, object>();

            // Assign fraction and int with probability.
            foreach (string var_name in Default_Ref_to_Number_keys)
            {
                if (rnd.NextDouble() < use_fraction)
                {
                    //真分数
                    //分母2,3,4,5
                    int denominator = rnd.Next(2,6);
                    int numerator = rnd.Next(1, denominator-1);
                    if (rnd.NextDouble() < 0.5)
                        Ref_to_Number[var_name] = new Number(numerator, denominator);
                    else
                        Ref_to_Number[var_name] = new Number(-numerator, denominator);
                }
                else
                {
                    //整数.
                    int value = rnd.Next(num_range_low, num_range_high);
                    Ref_to_Number[var_name] = new Number(value, 1);
                }
            }

            // adjustment for SB relationship
            foreach (Tuple<string, string> SB in SBpairs)
            {
                string small = SB.Item1;
                string big = SB.Item2;
                if ((Number)Ref_to_Number[small] > (Number)Ref_to_Number[big])
                {
                    //if violated, swap.
                    var tmp = Ref_to_Number[small];
                    Ref_to_Number[small] = Ref_to_Number[big];
                    Ref_to_Number[big] = tmp;
                }
                // if <=, do nothing
            }

            // adjustment for (small) Integar restriction
            foreach (string var_name in Int_Node)
            {
                //generate an integar. 0,1,2
                int value = rnd.Next(0, 2);
                Ref_to_Number[var_name] = new Number(value, 1);
            }

            return Ref_to_Number;
        }

        // facility:生成表达式符号字符串
        private static readonly Dictionary<char, int> op_priority = new Dictionary<char, int> {
            {'#', 0 },
            {'+', 1 },
            {'-', 1 },
            {'*', 2 },
            {'/', 2 },
            {'^', 3 }
        };
        public static string Tree_to_Symbol_string(Expression root, char base_op = '#')
        {
            if (root is VariableReference)
                return (root as VariableReference).GetName();
            else
            {
                Operation oper = root as Operation;
                char op = oper.GetOP();
                string result = 
                    $"{Tree_to_Symbol_string(oper.left, op)}" +
                    $" {op.ToString()} " +
                    $"{Tree_to_Symbol_string(oper.right, op)}";
                if (op_priority[oper.GetOP()] > op_priority[base_op])
                    return result;
                else return "(" + result + ")";
            }
        }

        //// CORE CODE: 返回*已经生成*的题目字符串
        public Tuple<List<string>, List<string>> Get_QA_pairs()
        {
            if (Ans.Count == 0)
            {
                throw new Exception("Empty expression list! Please call Generate before Save_to_file!");
            }
            return new Tuple<List<string>, List<string>>(Expression_in_number, Ans);
        }
    }
}
