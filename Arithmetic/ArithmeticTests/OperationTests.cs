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
    public class OperationTests
    {
        [TestMethod()]
        public void SwapBranchTest()
        {
            VariableReference a = new VariableReference("a");
            VariableReference b = new VariableReference("b");
            Operation op = new Operation(a, '+', b, 1);
            Operation op2 = new Operation(b, '+', a, 1);
            op2.SwapBranch();
            Operation op3 = new Operation(a, '+', b, 1);
            Operation op4 = new Operation(
                new VariableReference("a"), '+', new VariableReference("b"), 1);
            Assert.AreEqual(op, op2);
            Assert.AreEqual(op, op3);
            Assert.AreNotEqual(op, op4);

        }

        [TestMethod()]
        public void EvaluateTest()
        {
            Expression a = new VariableReference("a");
            Expression b = new VariableReference("b");
            Expression c = new VariableReference("c");

            Dictionary<string, object> Variable2Number = new Dictionary<string, object>();
            Variable2Number["a"] = new Number(3, 4);
            Variable2Number["b"] = new Number(7, 4);
            Variable2Number["c"] = new Number(-2, 1);

            Operation op = new Operation(a, '+', b, 1);
            Assert.AreEqual(op.Evaluate(Variable2Number), new Number(5, 2));

            Operation op2 = new Operation(a, '^', c, 1);
            Assert.AreEqual(op2.Evaluate(Variable2Number), new Number(16, 9));

        }
    }
}