using System;
using System.Text.RegularExpressions;

namespace ChallengeImpossible.Services
{
    public static class ValidationHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                     @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                     RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
