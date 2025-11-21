using Core.Model;
using Repo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Repo.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository()
            : base("Category")
        {           
        }

        protected override Category MapFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            DateTime? lastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt"));
            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

            string? categoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString(reader.GetOrdinal("CategoryName"));
            string? description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description"));

            return new Category(
                id,
                createdAt,
                lastUpdatedAt,
                isActive,
                categoryName,
                description
            );
        }

        protected override string BuildInsertSql(Category model)
        {
            // Adicionar parâmetros para INSERT e UPDATE
            string categoryName = model.CategoryName?.Replace("'", "''") ?? "NULL";
            string categoryDescription = model.Description?.Replace("'", "''") ?? "NULL";

            return $"INSERT INTO {_tableName} (CategoryName, Description, CreatedAt, IsActive) VALUES ('{categoryName}', '{categoryDescription}', GETDATE(), 1)";
        }

        protected override string BuildUpdateSql(Category model)
        {
            string categoryName = model.CategoryName?.Replace("'", "''") ?? "NULL";
            string categoryDescription = model.Description?.Replace("'", "''") ?? "NULL";

            return $"UPDATE {_tableName} SET CategoryName = '{categoryName}', Description = '{categoryDescription}', LastUpdatedAt = GETDATE() WHERE Id = {model.Id}";
        }

        public Category? GetByName(string categoryName)
        {
            string name = categoryName.Replace("'", "''");

            // O código SQL para esta função foi detalhado antes:
            string sql = $"SELECT * FROM {_tableName} Where CategoryName = '{name}' AND IsActive = 1";

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
                Console.WriteLine($"Erro no Repositório GETBYNAME: {ex.Message}");
                throw;
            }
            return default;
        }

        public bool ExistsWithName(string categoryName, int? excludeId = null)
        {
            string name = categoryName.Replace("'", "''");
            string sql = $"SELECT 1 FROM {_tableName} WHERE CategoryName = {name} AND IsActive = 1";

            if (excludeId.HasValue)
            {
                sql += " AND Id != {excludeId.Value}";
            }

            try
            {
                // ExecuteQuery aqui é menos eficiente que ExecuteScalar, mas usamos para manter a consistência da classe SQL.
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    return reader.HasRows;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no Repositório EXISTSWITHNAME: {ex.Message}");
                throw;
            }
        }
    }
}
