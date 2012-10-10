using System.Threading;

namespace QuizGameServer
{
    /// <summary>
    /// Manage game loop
    /// </summary>
    public class GameRunner
    {
        private readonly Timer _timer;
        private readonly QuizComunicator _controller;
        private readonly IQuestionProvider _provider;
        private readonly int _secondsToAnswer;

        public GameRunner(IQuestionProvider provider, IChatApi chatapi, int secondsToAnswer)
        {
            _provider = provider;
            _controller = new QuizComunicator(chatapi);
            _timer = new Timer(OnTimeout);
            _secondsToAnswer = secondsToAnswer;

            _controller.QuestionAnswered += _controller_QuestionAnswered;
        }

        void _controller_QuestionAnswered()
        {
            //TODO: Print stats
            PoseANewQuestion();
        }

        private void OnTimeout(object state)
        {
            PoseANewQuestion();
        }

        private void PoseANewQuestion()
        {
            QuestionItem question = null;
            if (_provider.TryGetQuestion(ref question))
            {
                _controller.PoseAQuestion(question);
            }
        }

        public void Start()
        {
            PoseANewQuestion();
            _timer.Change(_secondsToAnswer * 1000, Timeout.Infinite);
        }

        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
