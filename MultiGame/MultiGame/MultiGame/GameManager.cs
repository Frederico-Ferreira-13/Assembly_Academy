using System;
using System.Collections.Generic;
using System.Linq;
using MultiGame.Common;
using MultiGame.GameAbstract; // Para usar a classe base Game
using MultiGame.Games.TicTacToe; // Se tiver TicTacToe
using MultiGame.Games.ConnectFour; // Se tiver ConnectFour
using MultiGame.Games.NavalBattle;

namespace MultiGame.MultiGame
{
    public class GameManager
    {

        private string Player1Name;
        private string Player2Name;
        private Dictionary<string, List<Highscore>> allHighscores;

        public GameManager()
        {
            allHighscores = Highscore.LoadAllHighscores();
        }

        public void Run()
        {
            Console.WriteLine("Welcome to the MultiGame Arcade!");

            Console.Write("Enter Player 1 Name: ");
            Player1Name = Console.ReadLine();
            Console.Write("Enter Player 2 Name: ");
            Player2Name = Console.ReadLine();

            while (true)
            {
                int gameChoice = DisplayMainMenuAndGetChoice();

                if (gameChoice == 0)
                {
                    Console.WriteLine("Exiting... Goodbay!");
                    Highscore.SaveAllHighscores(allHighscores);
                    return;
                }
                else if (gameChoice == 4)
                {
                    DisplayAllHighscores();
                    Console.WriteLine("\nPress any key to return to menu.");
                    Console.ReadKey();
                }
                else
                {
                    Game gameToPlay = null;

                    switch (gameChoice)
                    {
                        case 1:
                            gameToPlay = new TicTacToe(Player1Name, Player2Name, allHighscores);
                            break;
                        case 2:
                            gameToPlay = new ConnectFour(Player1Name, Player2Name, allHighscores);
                            break;
                        case 3:
                            gameToPlay = new NavalBattleGameSession(Player1Name, Player2Name, allHighscores);
                            break;

                    }

                    if (gameToPlay != null)
                    {
                        gameToPlay.Play();
                    }
                    else
                    {
                        Console.WriteLine("Error: Could not start the selected game.");
                    }
                }
                Console.WriteLine("\nReturning to the game menu...");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }       

        private int DisplayMainMenuAndGetChoice()
        {
            int gameChoice;
            Console.Clear();
            Console.WriteLine("Choose a game: ");
            Console.WriteLine("1 - Tic Tac Toe (Jogo do Galo)");
            Console.WriteLine("2 - Connect Four (4 em Linha)");
            Console.WriteLine("3 - Naval Battle Game");
            Console.WriteLine("4 - View All Highscores");
            Console.WriteLine("0 - Exit");
            Console.Write("Select a game or option: ");
            // O Out "Como um modificador de parâmetro,
            // que permite que você passe um argumento para um método por referência em vez de por valor.
            // A outpalavra-chave é especialmente útil quando um método precisa 
            // retornar mais de um valor, pois mais de um outparâmetro pode ser usado, por exemplo
            while (!int.TryParse(Console.ReadLine(), out gameChoice) || gameChoice < 0 || gameChoice > 3)
            {
                Console.WriteLine("Invalid input. Please enter a valid choice (0-4)");
                Console.Write("Enter your choice: ");

            }
            return gameChoice;
        }

        private void DisplayAllHighscores()
        {
            Console.Clear();
            Console.WriteLine("--- All Games Highscores ---");
            if (allHighscores.Count == 0)
            {
                Console.WriteLine("No highscores recorded yet.");
                return;
            }

            foreach (KeyValuePair<string, List<Highscore>> entry in allHighscores)
            {
                string gameName = entry.Key;
                List<Highscore> highscores = entry.Value;

                Console.WriteLine($"\nGame: {gameName}");
                Console.WriteLine("--------------------");

                List<Highscore> sortedHighscores;
                
                if (gameName == "NavalBattleGame") // Lower score is better for Naval Battle (fewer turns)
                {
                    sortedHighscores = highscores.OrderBy(h => h.Score).ToList();
                }
                else // Higher score is better for other games (more wins/points)
                {
                    sortedHighscores = highscores.OrderByDescending(h => h.Score).ToList();
                }

                foreach (Highscore hs in sortedHighscores)
                {
                    Console.WriteLine($"  {hs.PlayerName}: {hs.Score} ({hs.Date.ToShortDateString()})");
                }
            }
            Console.WriteLine("----------------------------");
        }
    }
}
