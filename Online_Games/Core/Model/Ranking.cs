using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Ranking : BaseModel<int>
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int GameId { get; set; }      
        public Game? Game { get; set; }

        public int MMRValue { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public DateTime? LastUpdate { get; set; }

        public Ranking(int id, DateTime createdAt, DateTime? lastUpdatedAt, bool isActive,
                       int userId, int gameId, int mmrValue, int wins, int losses, DateTime? lastUpdate)
            : base(id, createdAt, lastUpdatedAt, isActive)
        {
            UserId = userId;
            GameId = gameId;
            MMRValue = mmrValue;
            Wins = wins;
            Losses = losses;
            LastUpdate = lastUpdate;
        }

        public Ranking(int id = 0) : base(id) 
        {
            MMRValue = 0;
            Wins = 0;
            Losses = 0;
        }

        public void UpdateMMRAndStats(int mmrChange, bool isWin)
        {
            this.MMRValue += mmrChange;
            if (isWin)
            {
                this.Wins++;
            }
            else
            {
                this.Losses++;
            }
            this.LastUpdate = DateTime.Now;
        }

        public string GetRankingSummary()
        {
            return $"Rank ID: {this.Id} | Jogador: {this.User?.UserName ?? "N/A"} | Jogo: {this.Game?.TypeGame ?? "N/A"} | MMR: {this.MMRValue} | W/L: {this.Wins}/{this.Losses} | Última Att: {this.LastUpdatedAt?.ToString("dd/MM/yyyy HH:mm")}";
        }
    }
}
