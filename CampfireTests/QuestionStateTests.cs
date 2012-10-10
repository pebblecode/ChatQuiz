using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizGameServer;

namespace CampfireTests
{
    [TestClass]
    public class QuestionStateTests
    {
        private QuestionState BuildQuestionState(QuestionItem question)
        {
            return new QuestionState(question, new FuzzyAnswerMatchStrategy(0));
        }

        [TestMethod]
        public void CheckAnswer_WrongAnswer_ReturnsFalse()
        {
            QuestionItem item = new QuestionItem { Question="1", Answer="42"};
            QuestionState state = BuildQuestionState(item);

            bool result = state.CheckAnswer("incorect answer");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckAnswer_CorrectAnswer_ReturnsTrue()
        {
            QuestionItem item = new QuestionItem { Question = "1", Answer = "42" };
            QuestionState state = BuildQuestionState(item);

            bool result = state.CheckAnswer("42");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsQuestionAnswered_WrongAnswer_ReturnsFalse()
        {
            QuestionItem item = new QuestionItem { Question = "1", Answer = "42" };
            QuestionState state = BuildQuestionState(item);
            state.CheckAnswer("blah");

            bool result = state.HasQuestionBeenAnswered;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsQuestionAnswered_CorrectAnswer_ReturnsTrue()
        {
            QuestionItem item = new QuestionItem { Question = "1", Answer = "42" };
            QuestionState state = BuildQuestionState(item);
            state.CheckAnswer("42");

            bool result = state.HasQuestionBeenAnswered;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsQuestionAnswered_NoAnswer_ReturnsFalse()
        {
            QuestionItem item = new QuestionItem { Question = "1", Answer = "42" };
            QuestionState state = BuildQuestionState(item);
          

            bool result = state.HasQuestionBeenAnswered;

            Assert.IsFalse(result);
        }
    
    
    
    }
}
