using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioRafael
{

    public class Alunos
    {
        private string[] alunos;

        public Alunos(int numeroDeAlunos)
        {
            alunos = new string[numeroDeAlunos];
        }
        public void DefinirNomeDoAluno(int indice, string nome)
        {
            if (indice >= 0 && indice < alunos.Length)
            {
                alunos[indice] = nome;
            }
            else
            {
                Console.WriteLine("Indice errado para definir o nome do aluno.");
            }
        }
        public string ObterNomeDoAluno(int indice)
        {
            if (indice >= 0 && indice < alunos.Length)
            {
                return alunos[indice];
            }
            return null;
        }

        public void ExibirListaDeAluno()
        {
            Console.WriteLine("Lista de alunos: ");
            for (int i = 0; i < alunos.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {alunos[i]}");
            }
        }
        public int NumeroDeAlunos => alunos.Length;


        
    }
}  



