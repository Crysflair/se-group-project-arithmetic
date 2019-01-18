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
    public class StandardizerTests
    {
        [TestMethod()]
        public void deci_2_fracTest()
        {
            Assert.AreEqual(Standardizer.deci_standardize("3", "5", true), "(-7/2)");
            Assert.AreEqual(Standardizer.deci_standardize("003", "500", false), "(7/2)");
            Assert.AreEqual(Standardizer.deci_standardize("101", "0000", false), "101");
            Assert.AreEqual(Standardizer.deci_standardize("01", "0000", true), "(-1)");
        }

        [TestMethod()]
        public void Standardize_NumberTest()
        {
            Assert.AreEqual(Standardizer.Standardize_Number("- 4 4 / 2"),"(-22)");
            Assert.AreEqual(Standardizer.Standardize_Number("- 1 0 . 2"), "(-51/5)");
            Assert.AreEqual(Standardizer.Standardize_Number("- 4"), "(-4)");
            Assert.AreEqual(Standardizer.Standardize_Number("4 / 3"), "(4/3)");
            Assert.AreEqual(Standardizer.Standardize_Number("6.25"), "(25/4)");
            Assert.AreEqual(Standardizer.Standardize_Number(" 666  "), "666");
            Assert.AreEqual(Standardizer.Standardize_Number("1,591.000,000,000"), "1591");


            Assert.AreEqual(Standardizer.Standardize_Number("+59"), "ERROR");
            Assert.AreEqual(Standardizer.Standardize_Number("77.5/3"), "ERROR");
            Assert.AreEqual(Standardizer.Standardize_Number("(-597)"), "ERROR");
            Assert.AreEqual(Standardizer.Standardize_Number("-.981"), "ERROR");
            Assert.AreEqual(Standardizer.Standardize_Number("982."), "ERROR");
            
        }
    }
}