using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace TaskManager.DataAccess.Infrastructure
{
    /// <summary>
    /// This is the data access implementation 
    /// using MySql as database and Dapper as ORM.
    /// </summary>
    public class DataAccessMySql : IDataAccess
    {
        // The connections strings of the database
        readonly string _connctionsStrings;

        /// <summary>
        /// DataAccessMySql constructor.
        /// using Dependency injection of the configuration
        /// to get the connections strings to the database 
        /// </summary>
        /// <param name="config">Dependency injection of the configuration</param>
        public DataAccessMySql(IConfiguration config)
        {
            _connctionsStrings = config.GetConnectionString("Cloud");
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T, U>(string sql, U parameters)
        {
            using IDbConnection connection = new MySqlConnection(_connctionsStrings);
            
            var records = await connection.QueryAsync<T>(sql, parameters);

            return records;
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T, U>(string sql, Func<T, U, T> func)
        {

            using IDbConnection connection = new MySqlConnection(_connctionsStrings);

            var records = await connection.QueryAsync(sql, func);

            return records;
        }

        public Task SeveData<T>(string sql, T parameters)
        {
            using IDbConnection connection = new MySqlConnection(_connctionsStrings);

            var sevaRecordTask = connection.ExecuteAsync(sql, parameters);

            return sevaRecordTask;
        }
    }
}
