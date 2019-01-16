﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic
{
    public class QuestionGenerator
    {

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

        // Constructor: check and save arguments
        public QuestionGenerator(
            int num_range_low, int num_range_high,        // 使用整数的数字范围
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

        // CORE CODE. This can be generated for multiple time!
        public void Generate(
                int generate_cnt,                             // 生成多少道题目
                string question_path, string answer_path)     // 题目和答案分别写入文件路径
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

                // then start generation !
                var SBpairs = generator.get_SBpairs();
                var Int_Node = generator.get_IntNodes();
                int symbol_to_number_failcnt = 0;
                while (generate_cnt > 0)
                {
                    // 生成一种合法的赋值
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
                        else continue;
                    }
                    else // if this expression in number is brand new
                    {
                        Expression_in_number.Add(expression_in_number);
                        Ans.Add(expression.Evaluate(Ref_to_Number).ToString());
                        generate_cnt--;
                    }
                }
            }

            // write to file TODO
            // Now is TESTING region ############
            foreach (string exp_symbol in Expression_in_symbol)
            {
                Console.WriteLine(exp_symbol);
            }Console.WriteLine("");

            foreach (string exp_number in Expression_in_number)
            {
                Console.WriteLine(exp_number);
            }
            Console.WriteLine("");
            foreach (string ans in Ans)
            {
                Console.WriteLine(ans);
            }
            Console.WriteLine("");
            // ################### TESTING region

        }

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

        // 生成合法赋值
        public static Dictionary<string, object> Assign_Ref_Value(
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
                    //generate a real fraction
                    //Note: the range of Numerator and Denominator are set to be the same as integar for convienience
                    int numerator = rnd.Next(num_range_high - num_range_low) + num_range_low;
                    int denominator = rnd.Next(num_range_high - num_range_low) + num_range_low;
                    Ref_to_Number[var_name] = new Number(numerator, denominator);
                }
                else
                {
                    //generate an integar.
                    int value = rnd.Next(num_range_high - num_range_low) + num_range_low;
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
                //generate an integar. -3 ~ 3
                int value = rnd.Next(7) -3;
                Ref_to_Number[var_name] = new Number(value, 1);
            }

            return Ref_to_Number;
        }

        // 生成表达式符号字符串
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
    }
}
