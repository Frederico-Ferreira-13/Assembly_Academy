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
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository() : base("[User]")
        {            
        }

        protected override User MapFromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
            DateTime? lastUpdatedAt = reader.IsDBNull(reader.GetOrdinal("LastUpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("LastUpdatedAt"));
            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
           
            string? pass = reader.IsDBNull(reader.GetOrdinal("Password")) ? null : reader.GetString(reader.GetOrdinal("Password"));
            string? email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email"));
            string? userName = reader.IsDBNull(reader.GetOrdinal("UserName")) ? null : reader.GetString(reader.GetOrdinal("UserName"));
            
            // O Mapeamento dos Enum's tem que se ler como um INT e fazer o Cast para UserRole
            UserRole role = (UserRole)reader.GetInt32(reader.GetOrdinal("Role"));
            
            bool IsApproved = reader.GetBoolean(reader.GetOrdinal("IsApproved"));


            return new User(
                id,
                createdAt,
                lastUpdatedAt,
                isActive,
                pass!,
                email!,
                userName!,
                role!,
                IsApproved
            );
        }

        protected override string BuildInsertSql(User model)
        {
            // Substituímos por SQL Bruta, escapando strings.
            string userPass = model.Pass.Replace("'", "''");
            string userEmail = model.Email.Replace("'", "''");
            string userUserName = model.UserName.Replace("'", "''");
            int roleInt = (int)model.Role;
            int isApprovedInt = model.IsApproved ? 1 : 0;

            return $"INSERT INTO {_tableName} (Password, Email, UserName, Role, IsApproved, CreatedAt, IsActive) " +
                   $"VALUES ('{userPass}', '{userEmail}', '{userUserName}', {roleInt}, {isApprovedInt}, GETDATE(), 1)";
        }

        protected override string BuildUpdateSql(User model)
        {
            string userPass = model.Pass.Replace("'", "''");
            string userEmail = model.Email.Replace("'", "''");
            string userUserName = model.UserName.Replace("'", "''");
            int roleInt = (int)model.Role;
            int isApprovedInt = model.IsApproved ? 1 : 0;

            return $"UPDATE {_tableName} SET Password = '{userPass}', Email = '{userEmail}', UserName = '{userUserName}', " +
                   $"Role = {roleInt}, IsApproved = {isApprovedInt}, LastUpdatedAt = GETDATE() " +
                   $"WHERE Id = {model.Id}";
        }

        public User? GetByEmail(string email)
        {
            string userEmail = email.Replace("'", "''");
            string sql = $"SELECT * FROM {_tableName} WHERE Email = '{userEmail}' AND IsActive = 1";

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
                Console.WriteLine($"\nErro SQL no Repositório GetByEmail: {ex.Message}");
                throw;
            }
            return null;
        }

        public bool ExistsByEmail(string email, int? excludeId = null)
        {
            string userEmail = email.Replace("'", "''");
            string sql = $"SELECT COUNT(1) FROM {_tableName} WHERE Email = '{userEmail}' AND IsActive = 1";

            if (excludeId.HasValue)
            {
                sql += $" AND Id != {excludeId.Value}";
            }

            try
            {
                // Como é um SELECT COUNT(1), usamos ExecuteQuery e lemos o resultado.
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    if (reader.Read())
                    {
                        // Assume que COUNT(1) é a primeira coluna (index 0)
                        return reader.GetInt32(0) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório ExistsByEmail: {ex.Message}");
                throw;
            }

            return false;
        }
        
        public List<User> GetByApprovalStatus(bool isApproved)
        {
            List<User> users = new List<User>();

            // 1 para true (approved), 0 para false (pendente)
            int IsApprovedInt = isApproved ? 1 : 0;

            string sql = $"SELECT * FROM {_tableName} WHERE IsApproved = {IsApprovedInt} AND IsActive = 1";

            try
            {
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        users.Add(MapFromReader(reader));
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório GetByApprovalStatus: {ex.Message}");
                throw;
            }

            return users;
        }
        
        public int CountAll()
        {
            string sql = $"SELECT COUNT(1) FROM {_tableName}";

            try
            {
                using(SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"\nErro SQL no Repositório CountAll: {ex.Message}");
                throw;
            }

            return 0;
        }
    }
}
