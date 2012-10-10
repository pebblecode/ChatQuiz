using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameServer
{
    public interface IAnswerMatchingStrategy
    {
        bool IsMatch(string p1, string p2);
    }
}
