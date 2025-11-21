using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiGame.Common;
using MultiGame.GameAbstract;

namespace MultiGame.Games.NavalBattle
{
    public class NavalBattleGame : Game
    {
        public NavalBattleGame(string player1Name, string player2Name, Dictionary<string, List<Highscore>> allHighscores)
            : base(player1Name, player2Name, allHighscores)
        {
        }
        public override void Play()
        {
            Console.WriteLine($"\nStarting Naval Battle with {Player1Name} and {Player2Name}");
            bool playAgain = true;

            while (playAgain)
            {
                Console.Clear();
                Console.WriteLine($"--- New Naval Battle Match ---");
                Console.WriteLine($"{Player1Name} will play against {Player2Name}.");

                NavalBattleGameSession gameSession = new NavalBattleGameSession(Player1Name, Player2Name, AllHighscores);

                string winner = gameSession.PlayAndReturnWinner();

                int turnsTaken = gameSession.TurnsTaken;

                if (!string.IsNullOrEmpty(winner))
                {
                    Console.WriteLine($"\n{winner} wins the Naval Battle!");
                    UpdateHighscores("NavalBatleGame", winner, turnsTaken);
                }
                else
                {
                    Console.WriteLine("\nNaval Battle ended without a winner (e.g., player quit).");
                }

                DisplayGameHighscores("NavalBatleGame");

                Console.Write("\nDo you want to play Naval Battle again? (yes/no): ");
                string playAgainInput = Console.ReadLine()!.ToLower();
                playAgain = playAgainInput == "yes" || playAgainInput == "y";
            }
            Console.WriteLine("Existing Naval Battle.");        
        }
    }
}