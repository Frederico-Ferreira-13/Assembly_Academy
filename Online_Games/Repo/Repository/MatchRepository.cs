using Core.Model;
using Microsoft.Data.SqlClient;
using Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repo.Repository
{
    public class MatchRepository : BaseRepository<Match>, IMatchRepository
    {
        public MatchRepository() : base("Match")
        {           
        }

        protected override Match MapFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            DateTime? lastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt"));
            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

            int tournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId"));
            int gameId = reader.GetInt32(reader.GetOrdinal("GameId"));
            int player1Id = reader.GetInt32(reader.GetOrdinal("Player1Id"));
            int player2Id = reader.GetInt32(reader.GetOrdinal("Player2Id"));
            int roundId = reader.GetInt32(reader.GetOrdinal("RoundId"));
            string? status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status"));
            int? winnerUserId = reader.IsDBNull(reader.GetOrdinal("WinnerUserId")) ? null : reader.GetInt32(reader.GetOrdinal("WinnerUserId"));
            int? looserUserId = reader.IsDBNull(reader.GetOrdinal("LooserUserId")) ? null : reader.GetInt32(reader.GetOrdinal("LooserUserId")); ;

            return new Match(
                id,
                createdAt,
                lastUpdatedAt,
                isActive,
                tournamentId,
                gameId,
                player1Id,
                player2Id,
                roundId,
                status,
                winnerUserId,
                looserUserId
            );
        }

        protected override string BuildInsertSql(Match model)
        {
            // Nota: Inteiros não precisam de aspas. Strings precisam de ser escapadas.
            string escapedStatus = model.Status?.Replace("'", "''") ?? "NULL";
            string winnerId = model.WinnerUserId.HasValue ? model.WinnerUserId.ToString() : "NULL";
            string looserId = model.LooserUserId.HasValue ? model.LooserUserId.ToString() : "NULL";

            return $"INSERT INTO {_tableName} (TournamentId, GameId, Player1Id, Player2Id, RoundId, Status, WinnerUserId, LooserUserId, CreatedAt, IsActive) " +
                   $"VALUES ({model.TournamentId}, {model.GameId}, {model.Player1Id}, {model.Player2Id}, {model.RoundId}, '{escapedStatus}', {winnerId}, {looserId}, GETDATE(), 1)";
        }

        protected override string BuildUpdateSql(Match model)
        {
            // Nota: Inteiros não precisam de aspas. Strings precisam de ser escapadas.
            string escapedStatus = model.Status?.Replace("'", "''") ?? "NULL";
            string winnerId = model.WinnerUserId.HasValue ? model.WinnerUserId.ToString() : "NULL";
            string looserId = model.LooserUserId.HasValue ? model.LooserUserId.ToString() : "NULL";

            return $"UPDATE {_tableName} SET " +
                   $"TournamentId = {model.TournamentId}, GameId = {model.GameId}, Player1Id = {model.Player1Id}, Player2Id = {model.Player2Id}, " +
                   $"RoundId = {model.RoundId}, Status = '{escapedStatus}', WinnerUserId = {winnerId}, LooserUserId = {looserId}, LastUpdatedAt = GETDATE() " +
                   $"WHERE Id = {model.Id}";
        }

        public List<Match> GetUpcomingMatches()
        {
            var matchs = new List<Match>();
            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1 AND Status = 'Scheduled' ORDER BY CreatedAt ASC";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        matchs.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetUpcomingMatches: {ex.Message}");
                throw;
            }
            return matchs;
        }

        public List<Match> GetRecentMatches(int count)
        {
            var matchs = new List<Match>();

            // Selecionamos os jogos com Status concluído e limitamos pelo topo (TOP @Count)
            string sql = $"SELECT TOP {count} * FROM {_tableName} WHERE IsActive = 1 AND Status = 'Completed' ORDER BY CreatedAt DESC";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        matchs.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetRecentMatches: {ex.Message}");
                throw;
            }
            return matchs;
        }        

        public List<Match> GetPlayerHistory(int userId)
        {
            var matchs = new List<Match>();

            // Filtramos por jogos concluídos OU que o utilizador participou e ordenamos do mais recente para o mais antigo.
            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1 AND Status = 'Completed' " +
                          $"AND (Player1Id = {userId} OR Player2Id = {userId}) " +
                          $"ORDER BY CreatedAt DESC";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        matchs.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetPlayerHistory: {ex.Message}");
                throw;
            }
            return matchs;
        }
    }    
}