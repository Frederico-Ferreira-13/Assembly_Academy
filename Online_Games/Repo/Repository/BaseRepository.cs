using Repo.Interfaces;
using Core.Model;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System;

namespace Repo.Repository
{

    // Repositório Genérico que implementa as operações CRUD básicas para qualquer modelo.
    // É uma classe abstrata, forçando as classes filhas a definir o mapeamento e parâmetros específicos.
    public abstract class BaseRepository<TModel> : IBaseRepository<TModel>
    where TModel : BaseModel<int>
    {
        protected readonly string _tableName;

        public BaseRepository(string tableName)
        {           
            _tableName = tableName;
        }

        // Método Abstrato para mapear um DataReader para um objeto TModel
        // Deve ser implementado nas classes filhas (ex: CategoryRepository)
        protected abstract TModel MapFromReader(SqlDataReader reader);
        protected abstract string BuildInsertSql(TModel model);
        protected abstract string BuildUpdateSql(TModel model);

        public void Create(TModel model)
        {
            string sql = BuildInsertSql(model);

            try
            {
                int newId = SQL.ExecuteNonQuery(sql);

                if(newId > 0)
                {
                    model.SetId(newId);
                }
                else
                {
                    throw new Exception("Falha ao obter o novo ID após a criação.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro no Repositório CREATE: {ex.Message}");
                throw;
            }
        }

        public TModel? Retrieve(int id)
        {
            string sql = $"SELECT * FROM {_tableName} Where Id = {id} AND IsActive = 1";

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
            catch(Exception ex)
            {
                Console.WriteLine($"Erro no Repositório RETRIEVE: {ex.Message}");
                throw;
            }

            return default;                
        }

        public List<TModel> RetrieveAll()
        {
            var models = new List<TModel>();
            string sql = $"SELECT * FROM {_tableName} WHERE IsActive = 1";

            try
            {
                // Chama o método estático para obter o DataReader.
                using (SqlDataReader reader = SQL.ExecuteQuery(sql))
                {
                    while (reader.Read())
                    {
                        models.Add(MapFromReader(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no Repositório RETRIEVEALL: {ex.Message}");
                throw;
            }
            return models;
        }         

        public void Update(TModel model)
        {
            string sql = BuildUpdateSql(model);

            try
            {
                // Chama o método estático.
                SQL.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no Repositório UPDATE: {ex.Message}");
                throw;
            }
        }

        public bool Delete(int id)
        {
            string sql = $"UPDATE {_tableName} SET IsActive = 0, LastUpdatedAt = GETDATE() WHERE Id = {id}";

            try
            {
                // Chama o método estático.
                int rowsAffected = SQL.ExecuteNonQuery(sql);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no Repositório DELETE: {ex.Message}");
                throw;
            }
        }       
    }
}
