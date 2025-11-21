using System;

namespace MultiGame.Games.NavalBattle
{
    public class GridUnit
    {
        public int Row { get; set; }
        public int Column { get; set; }       
       

        // Construtor que recebe a linha e acoluna
        public GridUnit(int row, int column)
        {
            Row = row;
            Column = column;
        }

        //Podemos adicionar métodos auxiliares se necessário, por exemplo, para comparar GridUnits
        //Importante para comparar instâncias de GridUnit (Verificar se duas uidades estão na mesma localização)
        // ou usá´las como chaves em coleções como dicionários ou conjuntos.
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            GridUnit other = (GridUnit)obj;
            return Row == other.Row && Column == other.Column;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
    }
}
