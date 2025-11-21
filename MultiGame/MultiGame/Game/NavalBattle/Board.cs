using System;
using System.Collections.Generic;
using System.Linq;
using MultiGame.Common;

namespace MultiGame.Games.NavalBattle
{
    public class Board
    {
        public Cell[,] Grid { get; private set; }
        public int Size { get; private set; }

        public Board(int size)
        {   Size = size;
            Grid = new Cell[size, size];

            // Inicializar todas as células do tabuleiro
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Grid[row, col] = new Cell();
                }
            }
        }

        //Método para verificar se um navio pode ser colocado numa determinada posição
        public bool CanPlaceShip(Ship ship, int startRow, int startCol, Orientation orientation)
        {
            if (ship == null)
            {
                throw new ArgumentNullException(nameof(ship), "Ship cannot be null.");
            }

            GridUnit[] potentialLocations = GetShipPlacementLocations(ship, startRow, startCol, orientation);

            foreach (var location in potentialLocations)
            {
                // verificar se alocalização está fora dos limites do tabuleiro.
                if (location.Row < 0 || location.Row >= Size ||
                    location.Column < 0 || location.Column >= Size)
                {
                    return false;
                }

                if (Grid[location.Row, location.Column].CurrentCell != CellState.Water)
                {
                    return false; // Célula já ocupada ou marcada como hit/miss
                }
            }
            return true;
        }

        public bool TryPlaceShip(Ship ship, int startRow, int startCol, char orientationChar)
        {
            Orientation orientation;
            if (orientationChar == 'H')
            {
                orientation = Orientation.Horizontal;
            }
            else if (orientationChar == 'V')
            {
                orientation = Orientation.Vertical;
            }
            else
            {
                return false;
            }
            if (CanPlaceShip(ship, startRow, startCol, orientation))
            {
                PlaceShipInternal(ship, startRow, startCol, orientation);
                return true;
            }
            return false;
        }

        private void PlaceShipInternal(Ship ship, int startRow, int startCol, Orientation orientation)
        {
            GridUnit[] locations = GetShipPlacementLocations(ship, startRow, startCol, orientation);
            for (int i = 0; i < ship.Length; i++)
            {
                Grid[locations[i].Row, locations[i].Column].CurrentCell = CellState.ShipUnit;
                Grid[locations[i].Row, locations[i].Column].ShipUnit = ship.ShipUnits[i];
                ship.ShipUnits[i].GridUnit = locations[i];
            }
        }

        public AttackResult AttackCell(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                return AttackResult.OutOfBounds;
            }

            Cell targetCell = Grid[row, col];

            if (targetCell.CurrentCell == CellState.Hit)
            {
                return AttackResult.AlreadyHit;
            }
            if (targetCell.CurrentCell == CellState.Miss)
            {
                return AttackResult.AlreadyMiss;
            }
            if (targetCell.CurrentCell == CellState.ShipUnit)
            {
                targetCell.MarkAsHit();
                return AttackResult.Hit;
            }
            else
            {
                targetCell.MarkAsMiss();
                return AttackResult.Miss;
            }
        }

        public bool IsCellAttacked(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
            {
                return false;
            }
            CellState state = Grid[row, col].CurrentCell;
            return state == CellState.Hit || state == CellState.Miss;
        }

        public CellState GetCellState(int row, int col)
        {
            if (row >= 0 && row < Size && col >= 0 && col < Size)
            {
                return Grid[row, col].CurrentCell;
            }
            return CellState.OutOfBounds; // Ou lançar uma excepção, dependendo do tratamento de erros
        }

        private GridUnit[] GetShipPlacementLocations(Ship ship, int startRow, int StartCol, Orientation orientation)
        {
            GridUnit[] locations = new GridUnit[ship.Length];
            for (int i = 0; i < ship.Length; i++)
            {
                int row = startRow + (orientation == Orientation.Vertical ? i : 0);
                int col = StartCol + (orientation == Orientation.Horizontal ? i : 0);
                locations[i] = new GridUnit(row, col);
            }
            return locations;
        }       

        public void DisplayBoard(bool showShips)
        {
            Console.Write("    ");
            for (int col = 0; col < Size; col++)
            {
                Console.Write($"{col + 1, -3} ");
                
            }
            Console.WriteLine();

            Console.Write("  ╔");
            for (int col = 0; col < Size; col++)
            {
                Console.Write("═══");
                if (col < Size - 1)
                {
                    Console.Write("╦");
                }
            }
            Console.WriteLine("╗");

            for (int row = 0; row < Size; row++)
            {
                char rowLetter = (char)('A' + row);

                Console.Write($"{rowLetter} ║");

                for (int col = 0; col < Size; col++)
                {
                    Cell currentCell = Grid[row, col];
                    char displayChar = ' ';
                    ConsoleColor color = ConsoleColor.Gray;

                    switch (currentCell.CurrentCell)
                    {
                        case CellState.Water:
                            displayChar = '~';
                            color = ConsoleColor.Cyan;
                            break;
                        case CellState.ShipUnit:                            
                            if(currentCell.ShipUnit != null && currentCell.ShipUnit.IsHit)
                            {
                                displayChar = 'X';
                                color = ConsoleColor.Red;
                            }                           
                            else
                            {
                                displayChar = '~';
                                color = ConsoleColor.Cyan;
                            }
                            break;
                        case CellState.Hit:
                            displayChar = 'X';
                            color = ConsoleColor.Red;
                            break;
                        case CellState.Miss:
                            displayChar = 'O';
                            color = ConsoleColor.White;
                            break;
                        case CellState.OutOfBounds:
                            displayChar = '?';
                            color = ConsoleColor.DarkGray;
                            break;
                    }

                    Console.ForegroundColor = color;
                    Console.Write($" {displayChar} ");
                    Console.ResetColor();
                    Console.Write("║");
                }
                Console.WriteLine();

                if (row < Size - 1)
                {
                    Console.Write("  ╠");
                    for (int col = 0; col < Size; col++)
                    {
                        Console.Write("═══");
                        if (col < Size - 1)
                        {
                            Console.Write("╬");
                        }
                    }
                    Console.WriteLine("╣");
                }
            }
            Console.Write("  ╚");
            for(int col = 0; col < Size; col++)
            {
                Console.Write("═══");
                if (col < Size - 1)
                {
                    Console.Write("╩");
                }
            }
            Console.WriteLine("╝");
        }
    }             
}
