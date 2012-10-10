using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizGameServer
{
    public interface IQuestionProvider
    {
        bool TryGetQuestion(ref QuestionItem question);
    }

    public interface IQuestionStore
    {
        void AddQuestion(QuestionItem questionItem);
    }

    /// <summary>
    /// This class buffers question data
    /// </summary>
    public class QueueQuestionRepository : IQuestionProvider, IQuestionStore
    {
        private readonly Lazy<Queue<QuestionItem>> _questions = new Lazy<Queue<QuestionItem>>();

        private QuestionItem GetQuestion()
        {
            if (_questions.Value.Count() == 0) throw new Exception();
            return _questions.Value.Dequeue();
        }

        public void AddQuestion(QuestionItem questionItem)
        {
            _questions.Value.Enqueue(questionItem);
        }

        public bool TryGetQuestion(ref QuestionItem question)
        {
            if (_questions.Value.Count() == 0) return false;
            question = GetQuestion();
            return true;
        }
    }
}
