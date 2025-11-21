using System;
using System.Linq;

namespace MultiGame.Games.NavalBattle
{
    public class Ship
    {
        public string Name { get; private set; }
        public int Length { get; private set; }        
        public List<ShipUnit> ShipUnits { get; private set; }

        public Ship(int length, string name)
        {                     
            Length = length;
            Name = name;
            ShipUnits = new List<ShipUnit>();

            for (int i = 0; i < Length; i++)
            {
                ShipUnits.Add(new ShipUnit());
            }
        }
        public bool IsSunk()
        {
            return ShipUnits.All(unit => unit.IsHit);
        }        
    }
}
