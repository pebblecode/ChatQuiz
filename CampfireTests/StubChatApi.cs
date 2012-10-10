using QuizGameServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampfireTests
{
    public class StubChatApi : IChatApi
    {
        private int postCount = 0;
        public int PostCount
        {
            get
            {
                return postCount;
            }
        }

        string _lastMessage;
        public void BroadcastMessage(string message)
        {
            _lastMessage = message;
            postCount++;
        }



        public void AnswerRecived(string message)
        {
            if (ChatMessage != null)
            {
                ChatMessage(message);
            }
        }


        public event ChatMessageReceived ChatMessage;

        public string LastMessage
        {
            get
            {
                return _lastMessage;
            }
        }
    }
}
