using Microsoft.VisualStudio.TestTools.UnitTesting;
using Arithmetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic.Tests
{
    [TestClass()]
    public class QuestionGeneratorTests
    {
        [TestMethod()]
        public void Tree_to_Symbol_stringTest()
        {
            // exp1
            Expression exp1_left = new Operation(
                new Operation(
                    new VariableReference("Fer"), '+', new VariableReference("Mys"), 1),
                '*', new VariableReference("Power"), 2);
            Expression exp1 = new Operation(
                exp1_left, '^', new VariableReference("Money"), 666666);
            Assert.AreEqual(QuestionGenerator.Tree_to_Symbol_string(exp1),
                "((Fer + Mys) * Power) ^ Money");

            // exp2
            Expression exp2 = new Operation(
                new VariableReference("a"), '+',
                new Operation(
                    new VariableReference("b"), '+',
                    new VariableReference("c"), 1
                    ),
                    2
                );
            Assert.AreEqual(QuestionGenerator.Tree_to_Symbol_string(exp2),
                "a + (b + c)");
        }

        [TestMethod()]
        public void Fill_numberTest()
        {
            char[] symbol_set = { '+', '-', '*', '/', '^' };
            string[] symbol_print = { "ADD", "SUB", "×", "÷", "**" };
            QuestionGenerator generator = new QuestionGenerator(0, 10, 0.5, 10, symbol_set, symbol_print);
            string expression_in_symbol = "a + (b + c) / d ^ e";
            Dictionary<string, object> Ref_to_Number = new Dictionary<string, object> {
                {"a", new Number(3,5)},
                {"b", new Number(-3,1)},
                {"c", new Number(2,1)},
                {"d", new Number(-4,1)},
                {"e", new Number(7,1)},
                {"zzzzz", new Number(2,1)},
            };
            Assert.AreEqual(generator.Fill_number(expression_in_symbol, Ref_to_Number),
                "(3/5) ADD ((-3) ADD 2) ÷ (-4) ** 7"
                );
        }
    }
}