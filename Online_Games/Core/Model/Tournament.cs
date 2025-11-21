using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Tournament : BaseModel<int>
    {       
        public DateTime Date { get; set; }
        public string? Type { get; set; }
        public string? Registration { get; set; }

        public string? Status { get; set; }
        public int? WinnerId { get; set; }

        public Tournament() : base(0) { }

        public Tournament(int id, DateTime date, string type, string registration, string status = "Scheduled", int? winnerId = null) : base(id)
        {
            Date = date;
            Type = type;
            Registration = registration;
            Status = status;
            WinnerId = winnerId;
        }

        public Tournament(int id, DateTime createdAt, DateTime? lastUpdatedAt, bool isActive, 
            DateTime date, string type, string registration, string status, int? winnerId)
            : base(id, createdAt, lastUpdatedAt, isActive)
        {
            Date = date;
            Type = type;
            Registration = registration;
            Status = status;
            WinnerId = winnerId;

        }

        public string GetTournamentSummary()
        {
            return $"Torneio ID: {this.Id} | Tipo: {this.Type ?? "N/A"} | Data: {this.Date.ToString("yyyy-MM-dd HH:mm")} | Estado: {this.Status ?? "N/A"} | Vencedor ID: {this.WinnerId ?? 0}";
        }
    }
}
