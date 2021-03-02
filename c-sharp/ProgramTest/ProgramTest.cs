using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace JokeGeneratorTest
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void TestMethod_ValidateInputNumberOfJokes_ShouldReturnIntegerWhenInputIsFrom1To9()
        {
            for (int i = 1; i < 10; i++)
            {
                Assert.AreEqual(i, Program.ValidateInputNumberOfJokes(i.ToString()));
            }

            var output = Program.ValidateInputNumberOfJokes("1.2");
            Assert.AreEqual(output, -1);

            output = Program.ValidateInputNumberOfJokes("0");
            Assert.AreEqual(output, -1);

            output = Program.ValidateInputNumberOfJokes("99");
            Assert.AreEqual(output, -1);

            output = Program.ValidateInputNumberOfJokes("a1");
            Assert.AreEqual(output, -1);

            output = Program.ValidateInputNumberOfJokes("-2");
            Assert.AreEqual(output, -1);
        }

        [TestMethod]
        public void TestMethod_FormatCategories_ShouldReturnHashsetWithCategories()
        {
            string[] testArray = new string[] { "[celebrity,celebrity,dev,explicit,fashion]" };
            HashSet<string> testHashSet = new HashSet<string> { "celebrity", "dev", "explicit", "fashion" };
            var output = Program.FormatCategories(testArray);
            Assert.IsTrue(output.SetEquals(testHashSet));
        }
    }
}