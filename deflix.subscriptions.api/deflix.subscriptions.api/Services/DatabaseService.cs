using System.Data;
using Dapper;
using deflix.subscriptions.api.Interfaces;
using Microsoft.Data.SqlClient;

namespace deflix.subscriptions.api.Services
{

    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }

        public int Execute(string query, object param = null)
        {
            return BeginAction(db => db.Execute(query, param));
        }

        public T QueryFirst<T>(string query, object param = null)
        {
            return BeginAction(db => db.QueryFirstOrDefault<T>(query, param));
        }

        public List<T> Query<T>(string query, object param = null)
        {
            return BeginAction(db => db.Query<T>(query, param)?.ToList());
        }

        private T BeginAction<T>(Func<IDbConnection, T> action)
        {
            try
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    if (db.State == ConnectionState.Closed) db.Open();
                    return action(db);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

}
