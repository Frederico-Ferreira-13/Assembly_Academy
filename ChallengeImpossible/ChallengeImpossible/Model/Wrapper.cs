using System;

namespace ChallengeImpossible.Model
{
    public class Wrapper
    {
        public static string ReadLineSafe()
        {
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
