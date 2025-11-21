using System;
using MultiGame.Common;

namespace MultiGame.Games.NavalBattle
{
    public class Cell
    {
        public CellState CurrentCell { get; set; } = CellState.Empty;
        public ShipUnit ShipUnit { get; set; } = null;

        public override string ToString()
        {
            switch (CurrentCell)
            {
                case CellState.Empty:
                    return "~";
                case CellState.ShipUnit:
                    return "S";
                case CellState.Hit:
                    return "X";
                case CellState.Miss:
                    return "O";
                default:
                    return "?";
            }
            return base.ToString();
        }

        public Cell()
        {
            CurrentCell = CellState.Water;
        }

        // Método para marcar a célula como atingida
        public void MarkAsHit()
        {
            if (CurrentCell == CellState.ShipUnit)
            {
                CurrentCell = CellState.Hit;
            }
        }

        public void MarkAsMiss()
        {
            if (CurrentCell == CellState.Water)
            {
                CurrentCell = CellState.Miss;
            }
        }
    }   
}
