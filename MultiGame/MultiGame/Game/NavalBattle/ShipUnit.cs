

namespace MultiGame.Games.NavalBattle
{
    public class ShipUnit
    {
        public GridUnit GridUnit { get; set; }
        public bool IsHit { get; private set; }

        // Construtor que recebe a linha e a coluna para inicializar a GridUnit
        public ShipUnit()
        {           
            IsHit = false;
        }       

        // Método para marcar esta unidade do navio como atingida
        public void Hit()
        {
            IsHit = true;
        }
    }
}
