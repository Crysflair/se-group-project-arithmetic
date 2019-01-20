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
    public class NumberTests
    {

        [TestMethod()]
        public void AddTest()
        {
            Assert.AreEqual(new Number(5, 1), new Number(3, 1).Add(new Number(2, 1)));
            Assert.AreEqual(new Number(5, 2), new Number(3, 2).Add(new Number(2, 2)));
            Assert.AreEqual(new Number(5, 6), new Number(2, 3).Add(new Number(1, 6)));
            Assert.AreEqual(new Number(15+18, 10), new Number(3, 2).Add(new Number(9, 5)));
            Assert.AreEqual(new Number(1, 1), new Number(3, 10).Add(new Number(7, 10)));
            Assert.AreEqual(new Number(0, 1), new Number(3, 2).Add(new Number(-6, 4)));
            Assert.AreEqual(new Number(1, 2), new Number(0, 1).Add(new Number(9, 18)));
            Assert.AreEqual(new Number(-1, 200), new Number(-7, 1000).Add(new Number(2, 1000)));

        }

        [TestMethod()]
        public void SubTest()
        {
            Assert.AreEqual(new Number(1, 2), new Number(5, 2).Sub(new Number(2, 1)));
            Assert.AreEqual(new Number(9, 5), new Number(5, 2).Sub(new Number(7, 10)));
            Assert.AreEqual(new Number(-10, 1), new Number(0, 1).Sub(new Number(20, 2)));
        }

        [TestMethod()]
        public void MulTest()
        {
            Assert.AreEqual(new Number(0, 1), new Number(0, 3).Mul(new Number(8, 9)));
            Assert.AreEqual(new Number(0, 1), new Number(8, 3).Mul(new Number(0, 9)));
            Assert.AreEqual(new Number(25, 36), new Number(5, 3).Mul(new Number(5, 12)));
            Assert.AreEqual(new Number(-25, 36), new Number(-5, 3).Mul(new Number(5, 12)));
            Assert.AreEqual(new Number(-25, 36), new Number(5, 3).Mul(new Number(-5, 12)));
            Assert.AreEqual(new Number(25, 36), new Number(-5, 3).Mul(new Number(-5, 12)));
        }

        [TestMethod()]
        public void DivTest()
        {
            Assert.AreEqual(new Number(0, 1), new Number(0, 5).Div(new Number(233, 250)));
            // TODO: add test to the exception detection of divide zero.
            Assert.AreEqual(new Number(0, 1), new Number(0, 5).Div(new Number(233, 250)));
            Assert.AreEqual(new Number(7, 30), new Number(1, 5).Div(new Number(6, 7)));
            Assert.AreEqual(new Number(-7, 30), new Number(-1, 5).Div(new Number(-6, -7)));
            Assert.AreEqual(new Number(7, 30), new Number(-1, 5).Div(new Number(-6, 7)));
        }

        [TestMethod()]
        public void PowTest()
        {   
            Assert.AreEqual(new Number(0, 1), new Number(0, 1).Pow(new Number(3, 1)));
            Assert.AreEqual(new Number(1, 1), new Number(0, 1).Pow(new Number(0, 3)));
            Assert.AreEqual(new Number(1, 1), new Number(24, 1).Pow(new Number(0, 1)));
            Assert.AreEqual(new Number(1, 8), new Number(1, 2).Pow(new Number(3, 1)));
            Assert.AreEqual(new Number(-1, 8), new Number(-1, 2).Pow(new Number(3, 1)));
            Assert.AreEqual(new Number(-8, 1), new Number(-1, 2).Pow(new Number(-3, 1)));
        }
    }
}