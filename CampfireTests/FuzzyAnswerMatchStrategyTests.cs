using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizGameServer;

namespace CampfireTests
{
    [TestClass]
    public class FuzzyAnswerMatchStrategyTests
    {
        [TestMethod]
        public void IsMatch_ExactMatch_ReturnsTrue()
        {
            FuzzyAnswerMatchStrategy strategy = new FuzzyAnswerMatchStrategy();

            bool result = strategy.IsMatch("STRING1", "STRING1");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatch_LevensteinDistance1_ReturnsTrue()
        {
            FuzzyAnswerMatchStrategy strategy = new FuzzyAnswerMatchStrategy(5);

            bool result = strategy.IsMatch("STRING2", "STRING1");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatch_LevensteinDistance5_ReturnTrue()
        {
            FuzzyAnswerMatchStrategy strategy = new FuzzyAnswerMatchStrategy(5);

            bool result = strategy.IsMatch("ABCDEF", "ABFEDC");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsMatch_LevensteinDistance6_ReturnFalse()
        {
            FuzzyAnswerMatchStrategy strategy = new FuzzyAnswerMatchStrategy(5);

            bool result = strategy.IsMatch("ABCDEFG", "APPPPPP");

            Assert.IsFalse(result);
        }
    }
}
