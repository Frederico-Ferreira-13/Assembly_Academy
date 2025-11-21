using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MultiGame.Common;
using MultiGame.GameAbstract;

namespace MultiGame.Games.ConnectFour
{
    public class ConnectFour : Game
    {
        private char[,] _board;
        private const int Rows = 6;
        private const int Cols = 7;

        public ConnectFour(string player1Name, string player2Name, Dictionary<string, List<Highscore>> allHighscores)
            : base(player1Name, player2Name, allHighscores)
        {
            _board = new char[Rows, Cols];
            InitializeBoard();
        }

        public override void Play()
        {
            Console.WriteLine($"Starting Connect Four with {Player1Name} and {Player2Name}");
            bool playAgain = true;

            while (playAgain)
            {
                InitializeBoard();
                int turn = 0;
                bool gameEnded = false;
                int currentPlayerIndex = 1;

                while (!gameEnded)
                {
                    Console.Clear();
                    DisplayBoard();

                    string currentPlayerName = currentPlayerIndex == 1 ? Player1Name : Player2Name;
                    char playerSymbol = currentPlayerIndex == 1 ? 'R' : 'B';
                    ConsoleColor playerColor = currentPlayerIndex == 1 ? ConsoleColor.Red : ConsoleColor.Blue;

                    Console.WriteLine($"{currentPlayerName}'s turn. Chose a column (1-{Cols}:");
                    string input = Console.ReadLine();
                    int column;

                    if (int.TryParse(input, out column) && column >= 1 && column <= Cols)
                    {
                        column--;

                        if (MakePlay(column, playerSymbol))
                        {
                            turn++;
                            int lastPlayedRow = GetLastPlayedRow(column);

                            if (CheckWin(lastPlayedRow, column, playerSymbol))
                            {
                                gameEnded = true;
                                Console.ForegroundColor = playerColor;
                                Console.WriteLine($"Congratulations! {currentPlayerName} wins!");
                                Console.ResetColor();
                                UpdateHighscores("ConnectFour", currentPlayerName, 1);
                                DisplayGameHighscores("ConnectFour");
                            }
                            else if (turn == Rows * Cols) // Condição de empate (tabuleiro cheio)
                            {
                                gameEnded = true;
                                Console.WriteLine("It's a Draw!");
                                DisplayGameHighscores("ConnectFour");
                            }
                            else
                            {
                                currentPlayerIndex = currentPlayerIndex == 1 ? 2 : 1;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid move. Column is full. Please choose another column.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid input. Please enter a column number from 1 to {Cols}.");
                        Console.ReadKey();
                    }
                }

                Console.WriteLine("\nConnect Four game ended.");
                Console.Write("Do you want to play again? (yes/no): ");
                string playAgainInput = Console.ReadLine()!.ToLower();
                playAgain = playAgainInput == "yes" || playAgainInput == "y";
            }
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    _board[row, col] = ' ';
                }
            }
        }

        private void DisplayBoard() // Mantendo o nome para consistência
        {
            Console.WriteLine("  1   2   3   4   5   6   7 ");
            Console.WriteLine("-----------------------------");
            for (int row = Rows - 1; row >= 0; row--) // Exibe de baixo para cima
            {
                for (int col = 0; col < Cols; col++)
                {
                    ConsoleColor cellColor = ConsoleColor.White;
                    if (_board[row, col] == 'R')
                        cellColor = ConsoleColor.Red;
                    else if (_board[row, col] == 'B')
                        cellColor = ConsoleColor.Blue;

                    Console.ForegroundColor = cellColor;
                    Console.Write($"| {_board[row, col]} ");
                    Console.ResetColor();
                }
                Console.WriteLine("|");
                Console.WriteLine("-----------------------------");
            }
        }

        private bool MakePlay(int column, char playerSymbol) // Mantendo o nome
        {
            for (int row = 0; row < Rows; row++)
            {
                if (_board[row, column] == ' ')
                {
                    _board[row, column] = playerSymbol;
                    return true;
                }
            }
            return false;
        }

        private int GetLastPlayedRow(int col)
        {
            for (int i = 0; i < Rows; i++)
            {
                if (_board[i, col] != ' ')
                {
                    return i;
                }
            }
            return -1; // Should not happen if MakePlay was successful
        }

        private bool CheckWin(int row, int col, char playerSymbol) // Usando a nova lógica
        {
            return CheckDirection(row, col, 0, 1, playerSymbol) ||
                    CheckDirection(row, col, 1, 0, playerSymbol) ||
                    CheckDirection(row, col, 1, 1, playerSymbol) ||
                    CheckDirection(row, col, 1, -1, playerSymbol);
        }

        private bool CheckDirection(int row, int col, int dr, int dc, char playerSymbol) // Nova lógica
        {
            int count = 0;

            for (int i = -3; i < 3; i++)
            {
                int r = row + i * dr;
                int c = col + i * dc;

                if (IsWithinBounds(r, c) && _board[r, c] == playerSymbol)
                {
                    count++;
                    if (count >= 4) return true;
                }
                else
                {
                    count = 0;
                }
            }
            return false;

        }
        private bool IsWithinBounds(int row, int col)
        {
            return row >= 0 && col >= 0 && row < Rows && col > Cols;
        }
    }
}
           