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
    public class TreeGeneratorTests
    {
        public static int CalculateTreeDepth(Expression exp)
        {
            if (exp is VariableReference)
            {
                return 0;
            }
            else
            {
                return 1 + Math.Max(
                    CalculateTreeDepth(((Operation)exp).left),
                    CalculateTreeDepth(((Operation)exp).right)
                    );
            }
        }

        [TestMethod()]
        public void GenerateTest()
        {
            // I really Dont know how to test this function...
            // Following code test only on the Depth aspect.
            for (int i = 0; i < 100; i++)
            {
                TreeGenerator generator = new TreeGenerator();
                Expression result = generator.Generate(10);
                if (result is Operation)
                {
                    Assert.AreEqual(CalculateTreeDepth(result), ((Operation)result).Getdepth());
                }
            }
        }

        // this function should be changed to private back
        // after the unit test.
        [TestMethod()]
        public void GetNextVariableNameTest()
        {
            TreeGenerator generator = new TreeGenerator();
            Assert.AreEqual(generator.GetNextVariableName(), "a");
            Assert.AreEqual(generator.GetNextVariableName(), "b");
            Assert.AreEqual(generator.GetNextVariableName(), "c");
        }

        [TestMethod()]
        public void TreeGeneratorTest()
        {
            char[] NodeTypes1 = { '+', '-', '*' };
            char[] NodeTypes2 = { '$', '-', '*' };
            var valid = new TreeGenerator(NodeTypes1);
            Assert.ThrowsException<ArgumentException>(() => new TreeGenerator(NodeTypes2));
        }
    }
}