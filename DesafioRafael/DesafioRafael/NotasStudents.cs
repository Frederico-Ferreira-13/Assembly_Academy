using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioRafael
{
    public class NotasStudents
    {
        public float[] nota1;
        public float[] nota2;
        public float[] nota3;

        public NotasStudents(int numeroDeAlunos)
        {
            nota1 = new float[numeroDeAlunos];
            nota2 = new float[numeroDeAlunos];
            nota3 = new float[numeroDeAlunos];
        }
        public void DefinirNotasStudents(int indiceAluno, float n1, float n2, float n3)
        {
            if(indiceAluno >= 0 && indiceAluno < nota1.Length)
            {
                nota1[indiceAluno] = n1;
                nota2[indiceAluno] = n2;
                nota3[indiceAluno] = n3;
            }
            else
            {
                Console.WriteLine("Indice de aluno inválido para definir notas.");
            }
        }

        public float CalcularMediaStudent(int indiceAluno)
        {
            if(indiceAluno >= 0 && indiceAluno < nota1.Length)
            {
                return (nota1[indiceAluno] + nota2[indiceAluno] + nota3[indiceAluno]) / 3;
            }
            Console.WriteLine("Indice de aluno inválido para calcular a média.");
            return -1;
        }

        public void ObterNotasStudent(int indiceAluno, out float nota1, out float nota2, out float nota3)
        {
            if(indiceAluno >= 0 && indiceAluno < this.nota1.Length)
            {
                nota1 = this.nota1[indiceAluno];
                nota2 = this.nota2[indiceAluno];
                nota3 = this.nota3[indiceAluno];
            }
            else
            {
                nota1 = -1;
                nota2 = -1;
                nota3 = -1;
                Console.WriteLine("índice de aluno inválido.");
            }
        }


    }
}
