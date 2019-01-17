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
    public class VariableReferenceTests
    {
        Dictionary<string, object> Variable2Number;
        public VariableReferenceTests()
        {
            Variable2Number = new Dictionary<string, object>();
            Variable2Number["a"] = new Number(3, 4);
            Variable2Number["b"] = new Number(0, 1);
            Variable2Number["c"] = new Number(1, 1);
            Variable2Number["d"] = new Number(-4, 5);
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            var Ref_a = new VariableReference("a");
            var Ref_b = new VariableReference("b");
            Assert.AreEqual(Ref_a.Evaluate(Variable2Number), new Number(3, 4));
            Assert.AreEqual(Ref_b.Evaluate(Variable2Number), Variable2Number["b"]);
        }
    }
}