using System;

namespace QuizGameServer
{
    public delegate void VoidEventHandler();

    /// <summary>
    /// Wires up question data to chat clients
    /// </summary>
    public class QuizComunicator
    {
        QuestionState _model;
        private readonly IChatApi _view;

        public event VoidEventHandler QuestionAnswered;

        public QuizComunicator(IChatApi chatapi)
        {
            _view = chatapi;
            _view .ChatMessage += OnChatMessageReceived;
        }

        void OnChatMessageReceived(ChatMessage possibleAnswer)
        {
            bool answeredCorrectly = _model.CheckAnswer(possibleAnswer.Message);
            HandleAnswer(answeredCorrectly);
        }

        public void PoseAQuestion(QuestionItem question)
        {
            if (question == null) throw new Exception();

            _view.BroadcastMessage(question.Question);
            _model = QuestionStateFactory.BuildQuestionState(question);
        }

        public bool IsQuestionAnswered
        {
            get
            {
                return _model.HasQuestionBeenAnswered;
            }
        }

        public void HandleAnswer(bool correctAnswer)
        {
            if (correctAnswer)
            {
                _view.BroadcastMessage("Correct Answer!");
                if (QuestionAnswered != null) QuestionAnswered();
            }
            else
            {
                _view.BroadcastMessage("Wrong Answer!");
            }
        }
    }
}
