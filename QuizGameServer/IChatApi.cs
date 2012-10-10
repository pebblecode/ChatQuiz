using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameServer
{
    public delegate void ChatMessageReceived(string message);

    public interface IChatApi
    {
        int PostCount { get; }

        void BroadcastMessage(string message);

        event ChatMessageReceived ChatMessage;
    }
}
