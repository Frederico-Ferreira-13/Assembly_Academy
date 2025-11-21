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
    public class RankingRepository : BaseRepository<Ranking>, IRankingRepository
    {
        public RankingRepository() : base("Ranking")
        {            
        }

        protected override Ranking MapFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            DateTime? lastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt"));
            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

            int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
            int gameId = reader.GetInt32(reader.GetOrdinal("GameId"));
            int mmrValue = reader.GetInt32(reader.GetOrdinal("MMRValue"));
            int wins = reader.GetInt32(reader.GetOrdinal("Wins"));
            int losses = reader.GetInt32(reader.GetOrdinal("Losses"));
            DateTime? lastUpdate = reader.IsDBNull(reader.GetOrdinal("LastUpdate")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdate"));
            

            return new Ranking(
                id,
                createdAt,
                lastUpdatedAt,
                isActive,
                userId,
                gameId,
                mmrValue,
                wins,
                losses,
                lastUpdate                
            );
        }

        protected override string BuildInsertSql(Ranking model)
        {
            // Inteiros e DateTime não precisam de aspas.
            string lastUpdate = model.LastUpdate.HasValue ? $"'{model.LastUpdate.Value:yyyy-MM-dd HH:mm:ss}'" : "NULL";

            return $"INSERT INTO {_tableName} (UserId, GameId, MMRValue, Wins, Losses, LastUpdate, CreatedAt, IsActive) " +
                   $"VALUES ({model.UserId}, {model.GameId}, {model.MMRValue}, {model.Wins}, {model.Losses}, {lastUpdate}, GETDATE(), 1)";
        }

        protected override string BuildUpdateSql(Ranking model)
        {
            string lastUpdate = model.LastUpdate.HasValue ? $"'{model.LastUpdate.Value:yyyy-MM-dd HH:mm:ss}'" : "NULL";

            return $"UPDATE {_tableName} SET UserId = {model.UserId}, GameId = {model.GameId}, MMRValue = {model.MMRValue}, " +
                   $"Wins = {model.Wins}, Losses = {model.Losses}, LastUpdate = {lastUpdate}, LastUpdatedAt = GETDATE() " +
                   $"WHERE Id = {model.Id}";
        }

        public List<Ranking> GetGlobalLeaderboardData(int limit)
        {
            var rankings = new List<Ranking>();

            // Agrupa por utilizador para obter o MMR médio ou total, mas para simplicidade, 
            // assumimos que MMRValue já está no contexto certo e só ordenamos.
            // Usamos TOP (@Limit) e ordenamos por MMRValue.
            string sql = $"SELECT TOP {limit} * FROM {_tableName} WHERE IsActive = 1 ORDER BY MMRValue DESC, Wins DESC";

            try
            {
                // Usamos o método estático do SQL
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        rankings.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetGlobalLeaderboardData: {ex.Message}");
                throw;
            }
            return rankings;
        }

        public List<Ranking> GetGameLeaderboardData(int gameId, int limit)
        {
            var rankings = new List<Ranking>();

            string sql = $"SELECT TOP {limit} * FROM {_tableName} WHERE IsActive = 1 AND GameId = {gameId} ORDER BY MMRValue DESC, Wins DESC";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        rankings.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetGameLeaderboardData: {ex.Message}");
                throw;
            }
            return rankings;
        }

        public Ranking? GetRankingByUserIdAndGameId(int userId, int gameId)
        {
            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1 AND UserId = {userId} AND GameId = {gameId}";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    if (reader.Read())
                    {
                        return MapFromReader(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n🚫 Erro SQL no Repositório GetRankingByUserIdAndGameId: {ex.Message}");
                throw;
            }
            return default;
        }

        public Ranking? GetUserRankingByGame(int userId, int gameId)
        {            
            return GetRankingByUserIdAndGameId(userId, gameId);
        }

        public List<Ranking> GetAllUserRankings(int userId)
        {
            var rankings = new List<Ranking>();

            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1 AND UserId = {userId} ORDER BY MMRValue DESC";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        rankings.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetAllUserRankings: {ex.Message}");
                throw;
            }
            return rankings;
        }

        public void RecalculateUserRanking(int winnerUserId, int looserUserId)
        {
            throw new NotImplementedException("A lógica de RecalculateUserRanking deve ser implementada na camada de Serviço, dependendo do GameId da partida.");
        }
    }
}
