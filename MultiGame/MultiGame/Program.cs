using System;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using MultiGame.MultiGame;

namespace MultiGame
{
    public class Program
    {
        static void Main(string[] args)
        {           

            GameManager gameManager = new GameManager();
            gameManager.Run();

            Console.WriteLine("Application has ended.");

        }
    }
}
