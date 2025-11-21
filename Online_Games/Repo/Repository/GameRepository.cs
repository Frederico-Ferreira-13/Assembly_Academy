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
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {       
        public GameRepository() : base("Game")
        {           
        }

        protected override Game MapFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            DateTime? lastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt"));
            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

            string? typeGame = reader.IsDBNull(reader.GetOrdinal("TypeGame")) ? null : reader.GetString(reader.GetOrdinal("TypeGame"));
            int categoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"));

            return new Game(
                id,
                createdAt,
                lastUpdatedAt,
                isActive,
                typeGame,
                categoryId
            );
        }

        protected override string BuildInsertSql(Game model)
        {
            // Adicionar parâmetros para INSERT e UPDATE
            string typeGame = model.TypeGame?.Replace("'", "''") ?? "NULL";
            int categoryId = model.CategoryId;

            return $"INSERT INTO {_tableName} (TypeGame, CategoryId, CreatedAt, IsActive) VALUES ('{typeGame}', '{categoryId}', GETDATE(), 1)";
        }

        protected override string BuildUpdateSql(Game model)
        {
            string typeGame = model.TypeGame?.Replace("'", "''") ?? "NULL";
            int categoryId = model.CategoryId;

            return $"UPDATE {_tableName} SET TypeGame = '{typeGame}', CategoryId = '{categoryId}', LastUpdatedAt = GETDATE() WHERE Id = {model.Id}";
        }

        public List<Game> Search(string searchTerm)
        {
            var games = new List<Game>();

            string gameSearchTerm = searchTerm?.Replace("'", "''") ?? "";
            
            // Usamos LIKE para pesquisa parcial e CONCAT para envolver o termo em '%'
            string sql = $"SELECT * FROM {_tableName} Where TypeGame LIKE '%'  + '{gameSearchTerm}' + '%' AND IsActive = 1";

            try
            {
                // Usamos o método estático do SQL
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        games.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório Search: {ex.Message}");
                throw;
            }
            return games;
        }

        public List<Game> GetByGenre(int genreId)
        {
            var games = new List<Game>();

            string sql = $"SELECT * FROM {_tableName} WHERE CategoryId = {genreId} AND IsActive = 1";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        games.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetByGenre: {ex.Message}");
                throw;
            }
            return games;
        }

        public bool IsTitleUnique(string title, int? excludeId = null)
        {
            string gameTitle = title?.Replace("'", "''") ?? "";
            string sql = $"SELECT * FROM {_tableName} WHERE TypeGame = '{gameTitle}' AND IsActive = 1";

            if (excludeId.HasValue)
            {
                sql += $" AND Id != {excludeId.Value}";
            }

            try
            {
                // ExecuteQuery aqui é menos eficiente que ExecuteScalar, mas usamos para manter a consistência da classe SQL.
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    // Verifica se o leitor encontrou alguma linha.
                    return reader.HasRows;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório IsTitleUnique: {ex.Message}");
                throw;
            }
        }
    }
}
