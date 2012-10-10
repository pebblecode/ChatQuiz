using CampfireHoon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameServer
{
    public class CampfireChatApiAdapter : IChatApi
    {
        CampfireRoom _room;
        public CampfireChatApiAdapter()
        {
            AccountConfig config = new AccountConfig { AccountName = "pebbleit", AuthToken = "fee994663c634db07ea450f0b1de0cdbbc583d61" };
            _room = new CampfireRoom(config, 536178, true);
            _room.DataEmitted += _room_DataEmitted;
        }

        void _room_DataEmitted(CampfireHoon.ChatMessage message)
        {
            if (ChatMessage != null)
            {
                ChatMessage(new ChatMessage { Message = message.Body, Username = message.Username });
            }
        }

        public void BroadcastMessage(string message)
        {
            _room.Say(message);
        }

        public event ChatMessageReceived ChatMessage;
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int timeToAnswer = 30;
            IChatApi chatClient = new CampfireChatApiAdapter();

            GameRunner runner = new GameRunner(GetDefaultQuestionProvider(), chatClient, timeToAnswer);
            runner.Start();

            Console.ReadLine();
            runner.Stop();
        }

        private static QueueQuestionRepository GetDefaultQuestionProvider()
        {
            QueueQuestionRepository questionProvider = new QueueQuestionRepository();

            questionProvider.AddQuestion(new QuestionItem {Question = "The Answer to Life the Universe and Everything!?", Answer = "42"});
            questionProvider.AddQuestion(new QuestionItem {Question = "In which organ of the body is insulin produced?", Answer = "Pancreas"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is the speed of sound at sea level called?", Answer = "MACH 1"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is aquaculture the scientific name for?", Answer = "Fish Farming"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is the boiling point of water in Farenheight?", Answer = "212"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What on an animal is a Scut?", Answer = "Tail"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What are the young of eels called?", Answer = "Elver"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What in medicine is Pathology?", Answer = "The Study of Diseases"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What type of animal is a corvid?", Answer = "A bird"});
            questionProvider.AddQuestion(new QuestionItem {Question = "How many Noble gases are there?", Answer = "Six"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is the largest type of shark?", Answer = "Whale shark"});
            questionProvider.AddQuestion(new QuestionItem {Question = "Which disease is characterised by spasmodic contraction of muscles is also called Lockjaw?", Answer = "tetanus"});
            questionProvider.AddQuestion(new QuestionItem {Question = "How many bits are there in a byte?", Answer = "Eight"});
            questionProvider.AddQuestion(new QuestionItem {Question = "The sternum is the medical name for what?", Answer = "Breastbone"});
            questionProvider.AddQuestion(new QuestionItem {Question = "In which organ of the body is the pineal gland?", Answer = "Brain"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What type of clock was first introduced by the US National Bureau of Standards in 1949?", Answer = "Atomic Clock (Mollecular}"});
            questionProvider.AddQuestion(new QuestionItem {Question = "Which organ is inflamed when one is suffering from Nephritis?", Answer = "Kidney"});
            questionProvider.AddQuestion(new QuestionItem {Question = "From the hue of which tree do the Blue Mountains in Australia receive their name?", Answer = "Eucalyptus"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is the better-known Australian relative of the cassowary?", Answer = "Emu"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What type of creature is a Basilisk?", Answer = "Lizard"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is the more common name for the Fireweed?", Answer = "Rose Bay willowherb"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is the common name for the Araucaria tree?", Answer = "Monkey Puzzle"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is Hypermetropia?", Answer = "Long Sightedness"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is the S.I. Unit of Force?", Answer = "Newton"});
            questionProvider.AddQuestion(new QuestionItem {Question = "W is the symbol for which chemical element?", Answer = "Tungsten"});
            questionProvider.AddQuestion(new QuestionItem {Question = "Which planet has an orbital period of 687 days?", Answer = "Mars"});
            questionProvider.AddQuestion(new QuestionItem {Question = "What is the more common term used to denote a temperature of nought degrees Kelvin?",Answer = "Absolute Zero"});
            questionProvider.AddQuestion(new QuestionItem {Question = "Who was the first pilot to exceed the speed of sound?", Answer = "Charles Yeager"});
            return questionProvider;
        }
    }

}
