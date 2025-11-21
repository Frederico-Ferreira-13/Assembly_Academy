using Repo.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Repo.Repository
{
    // Código refaturado utilizando na mesma a connectionstring e deixando de parte o Método prepere connection tanto
    // o ExecuteNonQuery como ExecuteQuery verificam a abertura da connection e também se existirá algum erro
    // tanto nas tabelas como na violação das Chaves Estrangeiras
    internal class SQL
    {
        const string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Online_Games;Integrated Security=True;TrustServerCertificate=True;Encrypt=False";

        public static int ExecuteNonQuery(string sql)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    
                    if (sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                    {
                        sql = sql + "; SELECT CAST(scope_identity() AS int)";
                        using(var sqlCommand = new SqlCommand(sql, conn))
                        {
                            var result = sqlCommand.ExecuteScalar();
                            return Convert.ToInt32(result);
                        }
                    }
                    else
                    {
                        using(var sqlCommand = new SqlCommand(sql, conn))
                        {
                            return sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nERRO FATAL no SQL.ExecuteNonQuery: {ex.Message}");
                    throw;
                }               
            }            
        }

        public static SqlDataReader ExecuteQuery(string sql)
        {
            var conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                var sqlCommand = new SqlCommand(sql, conn);
                
                return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n ERRO FATAL no SQL.ExecuteQuery: {ex.Message}");
                // Garante que fecha a conexão se falhar antes de retornar o reader
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();

                }
                throw;
            }
        }        
    }
}