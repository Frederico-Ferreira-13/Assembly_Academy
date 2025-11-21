using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System;

namespace DesafioRafael
{
    internal class Program
    {
        static Alunos turmaDeAlunos;
        static NotasStudents notasDeAlunos;

        static void InicializarDados()
        {
            int numeroDeAlunos = 5;
            turmaDeAlunos = new Alunos(numeroDeAlunos);
            notasDeAlunos = new NotasStudents(numeroDeAlunos);

            turmaDeAlunos.DefinirNomeDoAluno(0, "Maria Carolina");
            notasDeAlunos.DefinirNotasStudents(0, 13.4f, 17.5f, 18.7f);

            turmaDeAlunos.DefinirNomeDoAluno(1, "Ana Maria");
            notasDeAlunos.DefinirNotasStudents(1, 16.5f, 17.4f, 19.3f);

            turmaDeAlunos.DefinirNomeDoAluno(2, "João Paulo");
            notasDeAlunos.DefinirNotasStudents(2, 11.4f, 12.5f, 15.6f);

            turmaDeAlunos.DefinirNomeDoAluno(3, "José Manuel");
            notasDeAlunos.DefinirNotasStudents(3, 13.5f, 14.8f, 19.4f);

            turmaDeAlunos.DefinirNomeDoAluno(4, "Rodrigo Faria");
            notasDeAlunos.DefinirNotasStudents(4, 12.6f, 14.5f, 16.7f);
        }

        public static void Main(string[] args)
        {
            InicializarDados();

            int escolha = -1;
            while (escolha != 0)
            {
                Console.WriteLine("--- Menu de Gestão de Notas ---");
                Console.WriteLine("1 - Inserir dados dos alunos");
                Console.WriteLine("2 - Consultar aluno por nome");
                Console.WriteLine("3 - Mostrar média da turma");
                Console.WriteLine("4 - Mostrar melhor aluno");
                Console.WriteLine("5 - Mostrar lista de todos os alunos e respetivas médias");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                string escolhaStr = Console.ReadLine();
                if(int.TryParse(escolhaStr, out escolha))
                {
                    switch (escolha)
                    {
                        case 1:
                            Console.WriteLine("Opção 1 selecionada: Inserir dados dos alunos");
                            InserirDadosAlunos();
                            break;
                        case 2:
                            Console.WriteLine("Opção 2 selecionada: Consultar aluno por nome");
                            ConsultarAlunoPorNome();
                            break;
                        case 3:
                            Console.WriteLine("Opção 3 selecionada: Mostrar média da turma");
                            MostrarMediaTurma();
                            break;
                        case 4:
                            Console.WriteLine("Opção 4 selecionada: Mostrar melhor aluno");
                            MostrarMelhorAluno();
                            break;
                        case 5:
                            Console.WriteLine("Opção 5 selecionada: Mostrar lista de todos os alunos e respetivas médias");
                            MostrarListaMedias();
                            break;
                        case 0:
                            Console.WriteLine("Sair do programa!");
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Por favor, escolha uma opção do menu");
                            break;
                    }                
                }
                else
                {
                    Console.WriteLine("Opção inválida. Por favor, introduza uma opção do menu");
                }
                Console.WriteLine("\nPressione qualquer tecla para continuar.");
                Console.ReadKey();
                Console.Clear();
            }
            static void InserirDadosAlunos()
            {
                Console.WriteLine("\n--- Inserir/Modificar dados dos Alunos ---");
                
                Console.WriteLine("Introduza o número do aluno que deseja modificar (1 a 5 " + turmaDeAlunos.NumeroDeAlunos + "):");
                if (int.TryParse(Console.ReadLine(), out int numeroAluno) && numeroAluno >= 1 && numeroAluno <= turmaDeAlunos.NumeroDeAlunos)
                {
                    int indiceAluno = numeroAluno - 1;
                    Console.WriteLine($"Novo nome para {turmaDeAlunos.ObterNomeDoAluno(indiceAluno)}: ");
                    string novoNome = Console.ReadLine();
                    turmaDeAlunos.DefinirNomeDoAluno(indiceAluno, novoNome);

                    Console.WriteLine($"Introduza as 3 novas notas para {novoNome} (separadas por virgulas: ");
                    string[] notasStr = Console.ReadLine().Split(',');
                    if (notasStr.Length == 3 &&
                        float.TryParse(notasStr[0].Trim(), out float nota1) && nota1 >= 0 && nota1 <= 20 &&
                        float.TryParse(notasStr[1].Trim(), out float nota2) && nota2 >= 0 && nota2 >= 20 &&
                        float.TryParse(notasStr[2].Trim(), out float nota3) && nota3 >= 0 && nota3 <= 20)
                    {
                        notasDeAlunos.DefinirNotasStudents(indiceAluno, nota1, nota2, nota3);
                        Console.WriteLine("Dados do aluno atualizados com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Formato de notas inválido. As notas não foram atualizadas!");
                    }
                }
                else
                {
                    Console.WriteLine("Número de aluno inválido.");
                }
            }

            static void ConsultarAlunoPorNome()
            {
                Console.WriteLine("\n--- Consultar Aluno por Nome ---");
                Console.Write("Introduza o nome do aluno que deseja consultar: ");
                string nomeConsulta = Console.ReadLine();
                int indiceEncontrado = -1;
                for (int i = 0; i < turmaDeAlunos.NumeroDeAlunos; i++)
                {
                    if (turmaDeAlunos.ObterNomeDoAluno(i).Equals(nomeConsulta, StringComparison.OrdinalIgnoreCase))
                    {
                        indiceEncontrado = i;
                        break;
                    }
                }
                if (indiceEncontrado != -1)
                {
                    float nota1, nota2, nota3;
                    notasDeAlunos.ObterNotasStudent(indiceEncontrado, out nota1, out nota2, out nota3);
                    Console.WriteLine($"\nNotas de {turmaDeAlunos.ObterNomeDoAluno(indiceEncontrado)}:");
                    Console.WriteLine($"Nota 1: {nota1}");
                    Console.WriteLine($"Nota 2: {nota2}");
                    Console.WriteLine($"Nota 3: {nota3}");
                    Console.WriteLine($"Média: {notasDeAlunos.CalcularMediaStudent(indiceEncontrado): F2}");
                }
                else
                {
                    Console.WriteLine($"Aluno com o nome '{nomeConsulta}' não encontrado.");
                }
            }

            static void MostrarMediaTurma()
            {
                Console.WriteLine("\n--- Média Turma ---");
                float somaMedias = 0;

                for (int i = 0; i < turmaDeAlunos.NumeroDeAlunos; i++)
                {
                    somaMedias += notasDeAlunos.CalcularMediaStudent(i);                    
                }
                if(turmaDeAlunos.NumeroDeAlunos > 0)
                {
                    float mediaTurma = somaMedias / turmaDeAlunos.NumeroDeAlunos;
                    Console.WriteLine("A média da turma é: " + mediaTurma.ToString("F2"));
                }
                else
                {
                    Console.WriteLine("Não há alunos registados para calcular a média da turma.");
                }
            }

            static void MostrarMelhorAluno()
            {
                Console.WriteLine("\n--- Melhor Aluno ---");
                int indiceMelhorAluno = -1;
                float melhorMedia = -1;

                for (int i = 0; i < turmaDeAlunos.NumeroDeAlunos; i++)
                {
                    float mediaAluno = notasDeAlunos.CalcularMediaStudent(i);
                    if (mediaAluno > melhorMedia)
                    {
                        melhorMedia = mediaAluno;
                        indiceMelhorAluno = i;
                    }
                }
                if (indiceMelhorAluno != -1)
                {
                    Console.WriteLine($"O melhor aluno é: {turmaDeAlunos.ObterNomeDoAluno(indiceMelhorAluno)} com média {melhorMedia:F2}");
                }
                else
                {
                    Console.WriteLine("Não há alunos registados para terminar o melhor aluno.");
                }
            }
            static void MostrarListaMedias()
            {
                Console.WriteLine("\n--- Lista de Alunos e Médias ---");
                for (int i = 0; i < turmaDeAlunos.NumeroDeAlunos; i++)
                {
                    string nomeAluno = turmaDeAlunos.ObterNomeDoAluno(i);
                    float mediaAluno = notasDeAlunos.CalcularMediaStudent(i);
                    Console.WriteLine($"{nomeAluno}: {mediaAluno:F2}");
                }
            }

            float mediaMariaCarolina = notasDeAlunos.CalcularMediaStudent(0);
            Console.WriteLine($"A média de {turmaDeAlunos.ObterNomeDoAluno(0)} é: {mediaMariaCarolina} ");

            float mediaAnaMaria = notasDeAlunos.CalcularMediaStudent(1);
            Console.WriteLine($"A média de {turmaDeAlunos.ObterNomeDoAluno(1)} é: {mediaAnaMaria} ");

            float mediaJoaoPaulo = notasDeAlunos.CalcularMediaStudent(2);
            Console.WriteLine($"A média de {turmaDeAlunos.ObterNomeDoAluno(2)} é: {mediaJoaoPaulo} ");

            float mediaJoseManuel = notasDeAlunos.CalcularMediaStudent(3);
            Console.WriteLine($"A média de {turmaDeAlunos.ObterNomeDoAluno(3)} é: {mediaJoseManuel} ");

            float mediaRodrigoFaria = notasDeAlunos.CalcularMediaStudent(4);
            Console.WriteLine($"A média de {turmaDeAlunos.ObterNomeDoAluno(4)} é: {mediaRodrigoFaria}");
        }
    }
}

   