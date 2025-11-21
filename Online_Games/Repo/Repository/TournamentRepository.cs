using Core.Model;
using Microsoft.Data.SqlClient;
using Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Repo.Repository
{
    public class TournamentRepository : BaseRepository<Tournament>, ITournamentRepository
    {
        public TournamentRepository() : base("Tournament")
        {           
        }

        protected override Tournament MapFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            DateTime? lastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt"));
            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

            DateTime date = reader.GetDateTime(reader.GetOrdinal("Date"));
            string? type = reader.IsDBNull(reader.GetOrdinal("Type")) ? null : reader.GetString(reader.GetOrdinal("Type"));
            string? registration = reader.IsDBNull(reader.GetOrdinal("Registration")) ? null : reader.GetString(reader.GetOrdinal("Registration"));
            string? status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status"));            
            int? winnerId = reader.IsDBNull(reader.GetOrdinal("WinnerId")) ? null : reader.GetInt32(reader.GetOrdinal("WinnerId"));



            return new Tournament(
                id,
                createdAt,
                lastUpdatedAt,
                isActive,
                date,
                type!,
                registration!,
                status!,
                winnerId              
            );
        }

        protected override string BuildInsertSql(Tournament model)
        {
            // Formatação de data para SQL Server
            string date = $"'{model.Date:yyyy-MM-dd HH:mm:ss}'";
            string escapedType = model.Type?.Replace("'", "''") ?? "NULL";
            string escapedRegistration = model.Registration?.Replace("'", "''") ?? "NULL";
            string escapedStatus = model.Status?.Replace("'", "''") ?? "NULL";
            string winnerId = model.WinnerId.HasValue ? model.WinnerId.ToString() : "NULL";

            return $"INSERT INTO {_tableName} (Date, Type, Registration, Status, WinnerId, CreatedAt, IsActive) " +
                   $"VALUES ({date}, '{escapedType}', '{escapedRegistration}', '{escapedStatus}', {winnerId}, GETDATE(), 1)";
        }

        protected override string BuildUpdateSql(Tournament model)
        {
            string date = $"'{model.Date:yyyy-MM-dd HH:mm:ss}'";
            string escapedType = model.Type?.Replace("'", "''") ?? "NULL";
            string escapedRegistration = model.Registration?.Replace("'", "''") ?? "NULL";
            string escapedStatus = model.Status?.Replace("'", "''") ?? "NULL";
            string winnerId = model.WinnerId.HasValue ? model.WinnerId.ToString() : "NULL";

            return $"UPDATE {_tableName} SET Date = {date}, Type = '{escapedType}', Registration = '{escapedRegistration}', " +
                   $"Status = '{escapedStatus}', WinnerId = {winnerId}, LastUpdatedAt = GETDATE() " +
                   $"WHERE Id = {model.Id}";
        }

        public List<Tournament> GetUpcomingTournaments()
        {
            var tournaments = new List<Tournament>();

            // Filtra por MatchId e ordena pelo RoundNumber
            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1 AND Status = 'Scheduled' ORDER BY Date ASC";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        tournaments.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetUpcomingTournaments: {ex.Message}");
                throw;
            }
            return tournaments;
        }

        public List<Tournament> GetActiveTournaments()
        {
            var tournaments = new List<Tournament>();

            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1 AND (Status = 'In Progress' OR Status = 'Running') ORDER BY Date DESC";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        tournaments.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetActiveTournaments: {ex.Message}");
                throw;
            }
            return tournaments;
        }

        public bool IsPlayerRegistered(int tournamentId, int userId)
        {
            string sql = $"SELECT COUNT(1) FROM TournamentRegistrations WHERE TournamentId = {tournamentId} AND UserId = {userId}";

            try
            {
                // ExecuteScalar não é suportado pelo padrão SQL estático, mas podemos verificar o HasRows.
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    if (reader.Read())
                    {
                        // Se for COUNT(1), a primeira coluna (index 0) tem a contagem.
                        return reader.GetInt32(0) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n🚫 Erro SQL no Repositório IsPlayerRegistered: {ex.Message}");
                throw;
            }
            return false;
        }

        public void RegisterPlayer(int tournamentId, int userId)
        {
            string sql = $"INSERT INTO TournamentRegistrations (TournamentId, UserId, RegistrationDate) VALUES ({tournamentId}, {userId}, GETDATE())";

            try
            {
                // Usamos ExecuteNonQuery do SQL estático.
                SQL.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n🚫 Erro SQL no Repositório RegisterPlayer: {ex.Message}");
                throw;
            }
        }

        public List<int> GetRegisteredPlayerIds(int tournamentId)
        {
            var playerIds = new List<int>();

            string sql = $"SELECT UserId FROM TournamentRegistrations WHERE TournamentId = {tournamentId}";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        playerIds.Add(reader.GetInt32(0));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n🚫 Erro SQL no Repositório GetRegisteredPlayerIds: {ex.Message}");
                throw;
            }
            return playerIds;
        }

        public int? GetFinalWinner(int tournamentId)
        {
            // Este método pode ser mais simples se o vencedor estiver diretamente no modelo Tournament.
            // Aqui, usamos o método Retrieve() da BaseRepository e devolvemos o WinnerId.
            Tournament? tournament = Retrieve(tournamentId);

            return tournament?.WinnerId;
        }
    }
}
