using System;
using System.Collections.Generic;
using System.Linq;
using MultiGame.Common;

namespace MultiGame.GameAbstract
{
    public abstract class Game
    {
        public string Player1Name { get; protected set; }
        public string Player2Name { get; protected set; }
        public string GameName { get; protected set; }
        protected Dictionary<string, List<Highscore>> AllHighscores { get; set; }

        public Game(string player1Name, string player2Name, Dictionary<string, List<Highscore>> allHighscores)
        {
            Player1Name = player1Name;
            Player2Name = player2Name;
            AllHighscores = allHighscores;
        }

        public abstract void Play();

        protected virtual void UpdateHighscores(string gameName, string winnerName, int score)
        {
            if (!AllHighscores.ContainsKey(gameName))
            {
                AllHighscores[gameName] = new List<Highscore>();
            }

            AllHighscores[gameName].Add(new Highscore(winnerName, score));

            AllHighscores[gameName] = AllHighscores[gameName]
                .OrderByDescending(h => h.Score)
                .Take(10)
                .ToList();
            Console.WriteLine($"\nNew Highscore for {gameName}: {winnerName} - {score} points!");
        }

        protected void DisplayGameHighscores(string gameName)
        {
            Console.WriteLine($"\n--- Highscores for {gameName} ---");
            if (AllHighscores.ContainsKey(gameName) || !AllHighscores[gameName].Any())
            {
                Console.WriteLine("No highscores recorded for this game yet.");
                return;
            }
            List<Highscore> highscores = AllHighscores[gameName];

            if(gameName == "NavalBatleGame")
            {
                highscores = AllHighscores[gameName].OrderBy(h => h.Score).ToList();
            }
            else
            {
                highscores = AllHighscores[gameName].OrderByDescending(h => h.Score).ToList();
            }

            foreach(Highscore hs in highscores)
            {
                Console.WriteLine($" {hs.PlayerName}: {hs.Score} ({hs.Date.ToShortDateString()})");
            }
            Console.WriteLine("--------------------------------");
        }       
    }
}
