using System;
using System.Security.Cryptography;
using System.Text;

namespace ChallengeImpossible.Services
{
    public static class SecurePasswordHasher
    {
        public static string Hash(string plainTextPassword)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
            {
                throw new ArgumentNullException(nameof(plainTextPassword));
            }

            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
                var builder = new StringBuilder();
                foreach(var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool Verify(string plainTextPassword, string hash)
        {
            if(string.IsNullOrWhiteSpace(plainTextPassword) || string.IsNullOrWhiteSpace(hash))
            {
                return false;
            }
            return Hash(plainTextPassword) == hash;
        }
    }
}
