using Data.Interfaces;
using Core.Model;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Data.SqlClient;

namespace Data.Repository
{
    public abstract class BaseRepository<TModel> : IBaseRepository<TModel>
    where TModel : BaseModel<int>
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;

        public BaseRepository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }

        // Método Abstrato para mapear um DataReader para um objeto TModel
        // Deve ser implementado nas classes filhas (ex: CategoryRepository)
        protected abstract TModel MapFromReader(SqlDataReader reader);

        // Método Abstrato para adicionar os parâmetros de TModel ao SqlCommand
        protected abstract void AddParameters(SqlCommand command, TModel model);

        public void Create(TModel model)
        {
            string sql = $"INSERT INTO {_tableName} (Lista de Colunas) " +
                $"VALUES (Lista de @Parametros); " +
                $"SELECT SCOPE_IDENTITY();";

            using(var connection = new SqlConnection(_connectionString))
            using(var command = new SqlCommand(sql, connection))
            {
                AddParameters(command, model);

                connection.Open();

                object? newId = command.ExecuteScalar();

                if (newId != null && newId != DBNull.Value)
                {
                    model.Id = Convert.ToInt32(newId); // Atribui o ID gerado ao modelo
                }
            }            
        }

        public TModel? Retrieve(int id)
        {
            string sql = $"SELECT * FROM {_tableName} Where Id = @Id AND IsActive = 1";

            using(var connection = new SqlConnection(_connectionString))
            using(var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapFromReader(reader);
                    }
                }
            }

            return default;                
        }

        public List<TModel> RetrieveAll()
        {
            var models = new List<TModel>();
            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        models.Add(MapFromReader(reader));
                    }
                }
            }
            return models;
        }

        public void Update(TModel model)
        {
            string sql = $"UPDATE {_tableName} SET " +
                 // Colunas e Parâmetros devem ser listados aqui (ex: Name = @Name, Date = @Date, ...)
                 $"/* Lista de Coluna = @Parametro, ... */, LastUpdatedAt = GETDATE() " +
                 $"WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                // Adicionar o ID para o WHERE
                command.Parameters.AddWithValue("@Id", model.Id);

                // Adicionar todos os outros parâmetros
                AddParameters(command, model);

                connection.Open();

                // Executar e verificar se alguma linha foi afetada (opcional)
                command.ExecuteNonQuery();
            }
        }

        public bool Delete(int id)
        {
            string sql = $"UPDATE {_tableName} SET IsActive = 0, LastUpdatedAt = GETDATE() WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                // Devolve true se pelo menos uma linha foi atualizada.
                return rowsAffected > 0;
            }

            return true;
        }

        public Task<int> SaveChangesAsync()
        {
            return Task.FromResult(1); // Devolve 1 (representando 1 alteração)
        }
    }
}
