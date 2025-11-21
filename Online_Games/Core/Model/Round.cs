using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Round : BaseModel<int>
    {
        public int TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public int GameId { get; set; }
        public Game? Game { get; set; }

        public int MatchId { get; set; }
        public Match? Match { get; set; }

        public string? Country { get; set; }
        public int? RoundNumber { get; set; }       
       
        public List<int> WinnerIds { get; set; } = new List<int>();
        public List<int> LooserIds { get; set; } = new List<int>();

        public Round(int id, DateTime createdAt, DateTime? lastUpdatedAt, bool isActive, int tournamentId,
            int gameId, int matchId, string? country, int? roundNumber)
            : base(id, createdAt, lastUpdatedAt, isActive)
        {
            this.TournamentId = tournamentId;
            this.GameId = gameId;
            this.MatchId = matchId;
            this.Country = country;
            this.RoundNumber = roundNumber;
        }

        public Round(int tournamentId, int gameId, int matchId, string? country, int? roundNumber) : base(0) // Id=0 ou similar, passa 1 argumento para BaseModel
        {
            this.TournamentId = tournamentId;
            this.GameId = gameId;
            this.MatchId = matchId;
            this.Country = country;
            this.RoundNumber = roundNumber;
        }

        public Round(int id, DateTime createdAt, DateTime? lastUpdatedAt, bool isActive, int tournamentId,
            int gameId, string? country, int? roundNumber) : base(id, createdAt, lastUpdatedAt, isActive)
        {
            TournamentId = tournamentId;
            GameId = gameId;           
            Country = country;
            RoundNumber = roundNumber;
        }

        public string GetRoundSummary()
        {
            return $"Ronda ID: {this.Id} | Nº: {this.RoundNumber} | País: {this.Country ?? "N/A"} | Torneio ID: {this.TournamentId} | Jogo ID: {this.GameId} | Vencedores ({this.WinnerIds.Count}): {string.Join(", ", this.WinnerIds.Take(3).ToList())}...";
        }
    }
}