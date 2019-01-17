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
    public class QuestionProviderTests
    {
        [TestMethod()] // TODO: to test
        public void read_QA_pairs_from_filesTest()
        {
            string question_path = @"C:\Users\MysriO\Documents\Github Files\se-group-project-arithmetic\Arithmetic\Arithmetic\bin\Debug\questions.txt";
            string answer_path = @"C:\Users\MysriO\Documents\Github Files\se-group-project-arithmetic\Arithmetic\Arithmetic\bin\Debug\answers.txt";
            Tuple<List<string>, List<string>> QA_pairs = QuestionProvider.read_QA_pairs_from_files(4, question_path, answer_path);
            Assert.AreEqual(QA_pairs.Item1[0], "2 + ((3/2) + 2)");
            Assert.AreEqual(QA_pairs.Item1[3], "1 + (1 + 2)");
            Assert.AreEqual(QA_pairs.Item2[0], "(11/2)");
            Assert.AreEqual(QA_pairs.Item2[3], "4");
        }
    }
}