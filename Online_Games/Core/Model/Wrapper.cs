using System;

namespace Core.Model
{
    public class Wrapper
    {
        public static string ReadLineSafe()
        {
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
