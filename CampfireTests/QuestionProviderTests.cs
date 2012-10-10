using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizGameServer;

namespace CampfireTests
{
    [TestClass]
    public class QuestionProviderTests
    {
        [TestMethod]
        public void TryGetQuestion_NoQuestions_ReturnFalse()
        {
            QueueQuestionRepository provider = new QueueQuestionRepository();
            QuestionItem question = null;

            var result = provider.TryGetQuestion(ref question);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryGetQuestion_OneQuestions_ReturnsQuestion()
        {
            QueueQuestionRepository provider = new QueueQuestionRepository();
            provider.AddQuestion(new QuestionItem { Question = "Name?", Answer = "VS2012" });
            QuestionItem question = null;
            
            var result = provider.TryGetQuestion(ref question);

            Assert.IsTrue(result);
            Assert.IsNotNull(question);
        }

        [TestMethod]
        public void TryGetQuestion_TwoQuestions_FirstQuestionReturnedFirst()
        {
            QueueQuestionRepository provider = new QueueQuestionRepository();
            var q1 = new QuestionItem { Question = "1", Answer = "blah1" };
            var q2 = new QuestionItem { Question = "2", Answer = "blah2" };
            provider.AddQuestion(q1);
            provider.AddQuestion(q2);
            QuestionItem question1 = null;
            QuestionItem question2 = null;

            var result1 = provider.TryGetQuestion(ref question1);
            var result2 = provider.TryGetQuestion(ref question2);

            Assert.IsTrue(result1);
            Assert.AreEqual("1", question1.Question);
            Assert.AreEqual("blah1", question1.Answer);
        }

        [TestMethod]
        public void TryGetQuestion_TwoQuestions_SecondQuestionReturnedSecond()
        {
            QueueQuestionRepository provider = new QueueQuestionRepository();
            var q1 = new QuestionItem { Question = "1", Answer = "blah1" };
            var q2 = new QuestionItem { Question = "2", Answer = "blah2" };
            provider.AddQuestion(q1);
            provider.AddQuestion(q2);
            QuestionItem question1 = null;
            QuestionItem question2 = null;

            var result1 = provider.TryGetQuestion(ref question1);
            var result2 = provider.TryGetQuestion(ref question2);

            Assert.IsTrue(result2);
            Assert.AreEqual("2", question2.Question);
            Assert.AreEqual("blah2", question2.Answer);
        }
    }
}
