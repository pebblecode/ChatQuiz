using System;
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
            QuestionItem questionItem1 = new QuestionItem { Question = "Name1", Answer = "Alex1" };
            StubChatApi chatapi = new StubChatApi();
            QuizComunicator comunicator = new QuizComunicator(chatapi);
            comunicator.PoseAQuestion(questionItem1);

            ChatMessage message = new ChatMessage {Username = "JamesW", Message = "Alex1"};
            chatapi.AnswerRecived(message);

            Assert.IsTrue(comunicator.IsQuestionAnswered);
        }

        [TestMethod]
        public void OnChatMessageReceived_OneQuestionandCorrectAnswer_NotifySuccess()
        {
            QuestionItem questionItem1 = new QuestionItem { Question = "Name1", Answer = "Alex1" };
            StubChatApi chatapi = new StubChatApi();
            QuizComunicator comunicator = new QuizComunicator(chatapi);
            comunicator.PoseAQuestion(questionItem1);

            ChatMessage message = new ChatMessage { Username = "JamesW", Message = "Alex1" };
            chatapi.AnswerRecived(message);

            Assert.AreEqual("Correct Answer!", chatapi.LastMessage);
        }

        [TestMethod]
        public void OnChatMessageReceived_OneQuestionandIncorrectAnswer_NotifyWrongAnswer()
        {
            QuestionItem questionItem1 = new QuestionItem { Question = "Name1", Answer = "Alex1" };
            StubChatApi chatapi = new StubChatApi();
            QuizComunicator comunicator = new QuizComunicator(chatapi);
            comunicator.PoseAQuestion(questionItem1);

            ChatMessage message = new ChatMessage { Username = "JamesW", Message = "SOMETHING INCORRECT" };
            chatapi.AnswerRecived(message);

            Assert.AreEqual("Wrong Answer!", chatapi.LastMessage);
        }
    }
}
