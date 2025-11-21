using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiGame.Common
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }
    public enum CellState
    {
        Water,
        ShipUnit,
        Miss,
        Hit,
        OutOfBounds,
        Empty
    }
    public enum AttackResult
    {
        Miss,
        Hit,
        Sunk,
        AlreadyHit,
        AlreadyMiss,
        OutOfBounds
    }
}
