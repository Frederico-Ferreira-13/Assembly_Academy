using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MultiGame.Common;
using MultiGame.GameAbstract;

namespace MultiGame.Games.TicTacToe
{
    public class TicTacToe : Game
    {
        private char[,] _board;

        public TicTacToe(string player1Name, string player2Name, Dictionary<string, List<Highscore>> allHighscores)
            : base(player1Name, player2Name, allHighscores)
        {
            InitializeBoard();
        }

        public override void Play()
        {

            Console.WriteLine($"Starting Tic Tac Toe with {Player1Name} and {Player2Name}");
            bool playAgain = true;

            while (playAgain) // Loop para permitir multiplas partidas
            {
                InitializeBoard();

                int currentPlayerIndex = 1;
                char currentSymbol;
                bool gameEnded = false;
                int turnCount = 0;

                for (turnCount = 0; turnCount < 9 && !gameEnded; turnCount++)
                {
                    Console.Clear();
                    DisplayBoard();

                    string currentPlayerName = currentPlayerIndex == 1 ? Player1Name : Player2Name;
                    currentSymbol = currentPlayerIndex == 1 ? 'X' : 'O';

                    Console.WriteLine($"{currentPlayerName}'s turn ({currentSymbol}).");
                    MakePlay(currentSymbol);

                    Console.Clear();
                    DisplayBoard();

                    if (CheckWin(currentSymbol))
                    {
                        gameEnded = true;
                        Console.WriteLine($"Congratulations! The player {currentPlayerName} wins!");
                        UpdateHighscores("TicTacToe", currentPlayerName, 1);
                        DisplayGameHighscores("TicTacToe");
                        break;
                    }
                    currentPlayerIndex = currentPlayerIndex == 1 ? 2 : 1;
                }

                if (!gameEnded && turnCount == 9)
                {
                    Console.WriteLine("It's a draw!");
                    DisplayGameHighscores("TicTacToe");
                }

                Console.Write("Do you want to play again? (yes/no): ");
                string playAgainInput = Console.ReadLine()!.ToLower();
                playAgain = playAgainInput == "yes" || playAgainInput == "y";

            }
        }

        private void InitializeBoard()
        {
            _board = new char[3, 3]
            {// cordenadas ortogonais
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' },
                { ' ', ' ', ' ' }
            };
        }
        
        private void DisplayBoard()
        {
            // Exibir o tabuleiro do jogo formatado na consola
            Console.WriteLine();
            Console.WriteLine("  1 | 2 | 3 ");
            Console.WriteLine(" ---|---|---");
            Console.WriteLine($"  {_board[0, 0]} | {_board[0, 1]} | {_board[0, 2]}");
            Console.WriteLine(" ---|---|---");
            Console.WriteLine($"  {_board[1, 0]} | {_board[1, 1]} | {_board[1, 2]}");
            Console.WriteLine(" ---|---|---");
            Console.WriteLine($"  {_board[2, 0]} | {_board[2, 1]} | {_board[2, 2]}");
            Console.WriteLine("    |   |   ");
            Console.WriteLine();

        }
        private void MakePlay(char symbol)
        {
            int choice;
            bool validChoice = false;

            do
            {
                Console.Write("Choose a number (1-9): ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out choice) || choice < 1 || choice > 9)
                {
                    Console.WriteLine("invalid entry. Choose a number between 1 and 9.");
                    continue;
                }

                int row = (choice - 1) / 3;
                int col = (choice - 1) % 3;

                if (_board[row, col] == ' ')
                {
                    _board[row, col] = symbol;
                    validChoice = true;
                }
                else
                {
                    Console.WriteLine("That cell is already occupied. Choose another.");
                }
            } while (!validChoice);
        }
        private bool CheckWin(char symbol)
        {

            // verifica linhas, colunas para ver se um jogador venceu
            for (int i = 0; i < 3; i++)
            {
                if (_board[i, 0] == symbol && _board[i, 1] == symbol && _board[i, 2] == symbol)
                    return true; // Linha

                if (_board[0, i] == symbol && _board[1, i] == symbol && _board[2, i] == symbol)
                    return true; // Coluna
            }

            // Verifica as diagonais
            if (_board[0, 0] == symbol && _board[1, 1] == symbol && _board[2, 2] == symbol)
                return true;

            if (_board[0, 2] == symbol && _board[1, 1] == symbol && _board[2, 0] == symbol)
                return true;

            return false; // caso contrário, não há vencedor
        }
    }
}
