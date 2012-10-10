using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizGameServer;

namespace CampfireTests
{
    
    [TestClass]
    public class QuizComunicatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void PoseAQuestion_NullQuestionItem_ThrowsException()
        {
            QueueQuestionRepository provider = new QueueQuestionRepository();
            IChatApi chatapi = new StubChatApi();
            QuizComunicator comunicator = new QuizComunicator(chatapi);

            comunicator.PoseAQuestion(null);
        }

        [TestMethod]
        public void PoseAQuestion_OneQuestion_OnePost()
        {
            QuestionItem questionItem = new QuestionItem { Question = "Name", Answer = "Alex" };
            IChatApi chatapi = new StubChatApi();
            QuizComunicator comunicator = new QuizComunicator(chatapi);

            comunicator.PoseAQuestion(questionItem);

            Assert.AreEqual(1, chatapi.PostCount);
        }

        [TestMethod]
        public void OnChatMessageReceived_OneQuestionandCorrectAnswer_QuestionAnswered()
        {

            QueueQuestionRepository provider = new QueueQuestionRepository();
            QuestionItem questionItem1 = new QuestionItem { Question = "Name1", Answer = "Alex1" };
            StubChatApi chatapi = new StubChatApi();
            QuizComunicator comunicator = new QuizComunicator(chatapi);
            comunicator.PoseAQuestion(questionItem1);

            chatapi.AnswerRecived("Alex1");

            Assert.IsTrue(comunicator.IsQuestionAnswered);
        }

        [TestMethod]
        public void OnChatMessageReceived_OneQuestionandCorrectAnswer_NotifySuccess()
        {

            QueueQuestionRepository provider = new QueueQuestionRepository();
            QuestionItem questionItem1 = new QuestionItem { Question = "Name1", Answer = "Alex1" };
            StubChatApi chatapi = new StubChatApi();
            QuizComunicator comunicator = new QuizComunicator(chatapi);
            comunicator.PoseAQuestion(questionItem1);

            chatapi.AnswerRecived("Alex1");

            Assert.AreEqual("Correct Answer!", chatapi.LastMessage);
        }

        [TestMethod]
        public void OnChatMessageReceived_OneQuestionandIncorrectAnswer_NotifyWrongAnswer()
        {

            QueueQuestionRepository provider = new QueueQuestionRepository();
            QuestionItem questionItem1 = new QuestionItem { Question = "Name1", Answer = "Alex1" };
            StubChatApi chatapi = new StubChatApi();
            QuizComunicator comunicator = new QuizComunicator(chatapi);
            comunicator.PoseAQuestion(questionItem1);

            chatapi.AnswerRecived("SOMETHING INCORRECT");

            Assert.AreEqual("Wrong Answer!", chatapi.LastMessage);
        }
    }
}
