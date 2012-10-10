using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CampfireTests
{
    [TestClass]
    public class ScoreBoardTests
    {
        [TestMethod]
        public void GetUserScore_UserExistsWithScore0_ReturnsScore()
        {
            ScoreBoard board = new ScoreBoard();
            board.AddScore("JamesW", 0);

            int result =  board.GetUserScore("JamesW");

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetUserScore_UserExistsWithScore1ThenScores1_Returns2()
        {
            ScoreBoard board = new ScoreBoard();
            board.AddScore("JamesW", 1);
            board.AddScore("JamesW", 1);

            int result = board.GetUserScore("JamesW");

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetUserScore_UserDoesNotExist_ThrowException()
        {
            
        }
    }

    public class ScoreBoard
    {
        public void AddScore(string user, int score)
        {
            if(_scores.ContainsKey(user))
            {
                var currentScore = GetUserScore(user);
                _scores[user] = score + currentScore;
            }
            else
            {
                _scores.Add(user, score);
            }

        }

        private Dictionary<string, int> _scores = new Dictionary<string, int>();

        public int GetUserScore(string user)
        {
            if (_scores.ContainsKey(user))
            {
                int score;
                if (_scores.TryGetValue(user, out score))
                {
                    return score;
                }
                return 0;
            }
            throw new Exception("No Such User!");
        }
    }
}
