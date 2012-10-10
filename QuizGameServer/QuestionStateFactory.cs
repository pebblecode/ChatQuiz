using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameServer
{
    public class QuestionStateFactory
    {
        public static QuestionState BuildQuestionState(QuestionItem question)
        {
            return new QuestionState(question, new FuzzyAnswerMatchStrategy());
        }
    }
}
