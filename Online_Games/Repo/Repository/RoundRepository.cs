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
    public class RoundRepository : BaseRepository<Round>, IRoundRepository
    {
        public RoundRepository() : base("Round")
        {            
        }

        protected override Round MapFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            DateTime? lastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt"));
            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

            int tournamentId = reader.GetInt32(reader.GetOrdinal("TournamentId"));
            int gameId = reader.GetInt32(reader.GetOrdinal("GameId"));
            int matchId = reader.GetInt32(reader.GetOrdinal("MatchId"));

            string? country = reader.IsDBNull(reader.GetOrdinal("Country")) ? null : reader.GetString(reader.GetOrdinal("Country"));
            int? roundNumber = reader.IsDBNull(reader.GetOrdinal("RoundNumber")) ? null : reader.GetInt32(reader.GetOrdinal("RoundNumber"));
            
            return new Round(
                id,
                createdAt,
                lastUpdatedAt,
                isActive,
                tournamentId,
                gameId,
                matchId,
                country,
                roundNumber
            );
        }

        protected override string BuildInsertSql(Round model)
        {
            // Inteiros não precisam de aspas. Strings precisam de ser escapadas.
            string escapedCountry = model.Country?.Replace("'", "''") ?? "NULL";
            string roundNum = model.RoundNumber.HasValue ? model.RoundNumber.ToString() : "NULL";

            return $"INSERT INTO {_tableName} (TournamentId, GameId, MatchId, Country, RoundNumber, CreatedAt, IsActive) " +
                   $"VALUES ({model.TournamentId}, {model.GameId}, {model.MatchId}, '{escapedCountry}', {roundNum}, GETDATE(), 1)";
        }

        protected override string BuildUpdateSql(Round model)
        {
            string escapedCountry = model.Country?.Replace("'", "''") ?? "NULL";
            string roundNum = model.RoundNumber.HasValue ? model.RoundNumber.ToString() : "NULL";

            return $"UPDATE {_tableName} SET TournamentId = {model.TournamentId}, GameId = {model.GameId}, MatchId = {model.MatchId}, " +
                   $"Country = '{escapedCountry}', RoundNumber = {roundNum}, LastUpdatedAt = GETDATE() " +
                   $"WHERE Id = {model.Id}";
        }

        public List<Round> GetRoundsByMatch(int matchId)
        {
            var rounds = new List<Round>();

            // Filtra por MatchId e ordena pelo RoundNumber
            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1 AND MatchId = {matchId} ORDER BY RoundNumber ASC";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        rounds.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetRoundsByMatch: {ex.Message}");
                throw;
            }
            return rounds;
        }        
    }   
}
