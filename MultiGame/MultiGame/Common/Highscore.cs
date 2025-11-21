using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MultiGame.Common
{
    public class Highscore
    {
        private static readonly string HighscoresFilePath = "highscores.json";

        public string PlayerName { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }

        public Highscore(string playerName, int score)
        {
            PlayerName = playerName;
            Score = score;
            Date = DateTime.Now;
        }

        public Highscore() { }       

        public static Dictionary<string, List<Highscore>> LoadAllHighscores()
        {
            Dictionary<string, List<Highscore>> allHighscores = new Dictionary<string, List<Highscore>>();
            if (File.Exists(HighscoresFilePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(HighscoresFilePath);
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    return JsonSerializer.Deserialize<Dictionary<string, List<Highscore>>>(jsonString, options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading highscores: {ex.Message}");
                    return new Dictionary<string, List<Highscore>>();
                }
            }
            return new Dictionary<string, List<Highscore>>();
        }

        public static void SaveAllHighscores(Dictionary<string, List<Highscore>> allHighscores)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(allHighscores, options);
                File.WriteAllText(HighscoresFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving highscores: {ex.Message}");
            }
        }
    }
}
            
