using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizGameServer
{
    public delegate void VoidEventHandler();

    /// <summary>
    /// Wires up question data to chat clients
    /// </summary>
    public class QuizComunicator
    {
        private readonly IQuestionProvider provider;
        string _lastMessage = null;

        QuestionState model = null;
        private readonly IChatApi view;

        public event VoidEventHandler QuestionAnswered;

        public QuizComunicator(IChatApi chatapi)
        {
            this.provider = provider;
            this.view = chatapi;
            this.view .ChatMessage += OnChatMessageReceived;
        }

        void OnChatMessageReceived(string possibleAnswer)
        {
            _lastMessage = possibleAnswer;
            bool answeredCorrectly = model.CheckAnswer(possibleAnswer);
            HandleAnswer(answeredCorrectly);
        }

        public void PoseAQuestion(QuestionItem question)
        {
            if (question == null) throw new Exception();

            view.BroadcastMessage(question.Question);
            model = QuestionStateFactory.BuildQuestionState(question);
        }

        public bool IsQuestionAnswered
        {
            get
            {
                return model.HasQuestionBeenAnswered;
            }
        }

        public void HandleAnswer(bool correctAnswer)
        {
            if (correctAnswer)
            {
                view.BroadcastMessage("Correct Answer!");
                if (QuestionAnswered != null) QuestionAnswered();
            }
            else
            {
                view.BroadcastMessage("Wrong Answer!");
            }
        }
    }
}
