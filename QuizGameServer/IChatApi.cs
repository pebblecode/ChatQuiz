using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameServer
{
    public delegate void ChatMessageReceived(ChatMessage message);

    public class ChatMessage
    {
        public string Username
        {
            get; set;
        }

        public string Message { get; set; }
    }

    public interface IChatApi
    {
        int PostCount { get; }

        void BroadcastMessage(string message);

        event ChatMessageReceived ChatMessage;
    }
}
