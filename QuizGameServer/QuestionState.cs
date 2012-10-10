using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameServer
{
    /// <summary>
    /// Handles Qustion State Changes 
    /// </summary>
    public class QuestionState
    {
        private QuestionItem item;
        private bool _questionState = false;
        private IAnswerMatchingStrategy _matchingStrategy;

        public QuestionState(QuestionItem item, IAnswerMatchingStrategy matchingStrategy)
        {
            this.item = item;
            _matchingStrategy = matchingStrategy;
        }

        public bool CheckAnswer(string possibleAnswer)
        {
            
            if (_matchingStrategy.IsMatch(item.Answer, possibleAnswer))
            {
                HasQuestionBeenAnswered = true;
                return true;
            }
            
            return false;
        }

        public bool HasQuestionBeenAnswered { get { return _questionState; } private set { _questionState = value; } }
    }
}
