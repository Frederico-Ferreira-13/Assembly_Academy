using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiGame.Common;
using MultiGame.GameAbstract;

namespace MultiGame.Games.NavalBattle
{
    public class NavalBattleGameSession : Game
    {
        private Board _player1Board;
        private List<Ship> _player1Ships;
        private Board _player2Board;
        private List<Ship> _player2Ships;

        private const int BoardSize = 10;

        private static readonly (string name, int length)[] ShipDefinitions = new[]
        {
            ("Carrier", 5),
            ("Battleship", 4),
            ("Destroyer", 3),
            ("Submarine", 3),
            ("Patrol Boat", 2)
        };       

        private Random _random = new Random();

        public int TurnsTaken { get; private set; }

        public NavalBattleGameSession(string player1Name, string player2Name, Dictionary<string, List<Highscore>> allHighscores)
            : base(player1Name, player2Name, allHighscores)
        {
            GameName = "NavalBattleGame";
            _player1Board = new Board(BoardSize);
            _player2Board = new Board(BoardSize);
            _player1Ships = new List<Ship>();
            _player2Ships = new List<Ship>();
        }

        public override void Play()
        {
            PlayAndReturnWinner();
        }

        public string PlayAndReturnWinner()
        {
            Console.Clear();
            Console.WriteLine($"--- Starting {GameName} ---");
            Console.WriteLine($"{Player1Name} Vs {Player2Name}");

            Console.WriteLine($"\n{Player1Name}, prepare your fleet!");
            InitializePlayer(_player1Board, _player1Ships);
            Console.WriteLine($"\n{Player1Name}, your board is ready:");
            _player1Board.DisplayBoard(false);

            Console.WriteLine("\nPress any key for Player 2 to prepare.");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine($"\n{Player2Name}, prepare your fleet!");
            InitializePlayer(_player2Board, _player2Ships);
            Console.WriteLine($"\n{Player2Name}, your board is ready:");
            _player2Board.DisplayBoard(false);

            Console.WriteLine("\nPress any key for Player 2 to prepare.");
            Console.ReadKey();
            Console.Clear();

            int turns = 0;
            string currentPlayerName = "";
            Board currentPlayerBoard;
            Board enemyBoard;
            List<Ship> enemyShips;

            bool gameOver = false;
            string? winnerName = null;

            while (!gameOver)
            {
                turns++;

                if (turns % 2 != 0)
                {
                    currentPlayerName = Player1Name;
                    currentPlayerBoard = _player1Board;
                    enemyBoard = _player2Board;
                    enemyShips = _player2Ships;
                }
                else
                {
                    currentPlayerName = Player2Name;
                    currentPlayerBoard = _player2Board;
                    enemyBoard = _player1Board;
                    enemyShips = _player1Ships;
                }

                Console.Clear();
                Console.WriteLine($"--- Turn {turns} - It's {currentPlayerName}'s turn ---");

                Console.WriteLine($"\n{currentPlayerName}'s Board (Your Ships):");
                currentPlayerBoard.DisplayBoard(false);

                Console.WriteLine($"\n{currentPlayerName}'s Target Board (Enemy Waters):");
                enemyBoard.DisplayBoard(false);

                PerformAttack(currentPlayerBoard, enemyBoard, enemyShips);

                if (CheckWinCondition(enemyShips))
                {
                    gameOver = true;
                    winnerName = currentPlayerName;
                    Console.WriteLine($"\n--- Game Over! {currentPlayerBoard} wins in {turns} turns! ---");                    
                }

                if (!gameOver)
                {
                    Console.WriteLine("\nPress any key to end turn.");
                    Console.ReadKey();
                }
            }

            TurnsTaken = turns;          

            return winnerName;
        }
        
        private void InitializePlayer(Board board, List<Ship> ships)
        {
            for(int r = 0; r < BoardSize; r++)
            {
                for(int c = 0; c < BoardSize; c++)
                {
                    board.Grid[r, c] = new Cell();
                }
            }
            ships.Clear();

            foreach((string name, int length) shipDefinition in ShipDefinitions)
            {
                Ship newShip = new Ship(shipDefinition.length, shipDefinition.name);
                ships.Add(newShip);
                PlaceShipRandomly(board, newShip);
            }
        }      

        private void PlaceShipRandomly(Board board, Ship ship)
        {
            bool placed = false;
            while (!placed)
            {
                int row = _random.Next(BoardSize);
                int col = _random.Next(BoardSize);

                char orientationChar = _random.Next(2) == 0 ? 'H' : 'V';

                if (board.TryPlaceShip(ship, row, col, orientationChar))
                {
                    placed = true;
                }
            }
        }
        
        private void PerformAttack(Board playerBoard, Board enemyBoard, List<Ship> enemyShips)
        {
            bool validAttack = false;
            while (!validAttack)
            {
                Console.Write("Enter target row (A-J): ");
                char rowChar = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();

                Console.Write("Enter target column (1-10): ");
                int col;
                while (!int.TryParse(Console.ReadLine(), out col) || col < 1 || col > BoardSize)
                {
                    Console.Write("Invalid column. Enter a number between 1 and 10: ");
                }

                int row = rowChar - 'A';
                col--;

                if(row < 0 || row >= BoardSize)
                {
                    Console.WriteLine("Invalid row. Try again.");
                    continue;
                }

                if(enemyBoard.IsCellAttacked(row, col))
                {
                    Console.WriteLine("You've already attacked this cell. Choose another.");
                    continue;
                }

                AttackResult result = enemyBoard.AttackCell(row, col);
                Ship sunkShip = null;
                ProcessAttackResult(result, enemyShips, row, col, out sunkShip);
                validAttack = true;
            }
        }

        private void ProcessAttackResult(AttackResult result, List<Ship> enemyShips, int row, int col, out Ship sunkShip)
        {
            sunkShip = null;

            switch (result)
            {
                case AttackResult.Hit:
                    Console.WriteLine("HIT!");
                    Ship hitShip = FindShipAtLocation(enemyShips, row, col);
                    if(hitShip != null)
                    {
                        var hitUnit = hitShip.ShipUnits.FirstOrDefault(unit => unit.GridUnit != null && 
                        unit.GridUnit.Row == row && unit.GridUnit.Column == col);
                        if(hitUnit != null)
                        {
                            hitUnit.Hit();
                        }

                        if (hitShip.IsSunk())
                        {
                            Console.WriteLine($"You sunk enemy's {hitShip.Name}");
                            sunkShip = hitShip;
                        }                      
                    }
                    break;
                case AttackResult.Miss:
                    Console.WriteLine("MISS!");
                    break;
                case AttackResult.AlreadyMiss:
                    Console.WriteLine("Already attacked this cell.");
                    break;
                case AttackResult.OutOfBounds:
                    Console.WriteLine("That's outside the board! Try again.");
                    break;
                default:
                    Console.WriteLine("An unexpected attack result occurred.");
                    break;
            }
        }

        private Ship FindShipAtLocation(List<Ship> fleet, int row, int col)
        {
            foreach (Ship ship in fleet)
            {
                if (ship.ShipUnits != null && ship.ShipUnits.Any(unit => unit.GridUnit != null && unit.GridUnit.Row == row && unit.GridUnit.Column == col))                
                {
                    return ship;
                }
            }
            return null;
        }

        private bool CheckWinCondition(List<Ship> enemyShips)
        {
            return !enemyShips.Any();
        }

        protected override void UpdateHighscores(string gameName, string winnerName, int turnsTaken)
        {
            if (!AllHighscores.ContainsKey(gameName))
            {
                AllHighscores[gameName] = new List<Highscore>();
            }

            AllHighscores[gameName].Add(new Highscore(winnerName, turnsTaken));

            AllHighscores[gameName] = AllHighscores[gameName]
                .OrderBy(h => h.Score)
                .Take(10)
                .ToList();

            Console.WriteLine($"\nNew Highscore for {gameName}: {winnerName} - {turnsTaken} turns!");
        }          
    }
}
