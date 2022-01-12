using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.DataAccess.Infrastructure
{
    public interface IDataAccess
    {
        /// <summary>
        /// Load data from the database using sql query and dynamic parameters.
        /// </summary>
        /// <typeparam name="T">Type of the result</typeparam>
        /// <typeparam name="U">Type of the parameters. Should be dynamic for 'get by'</typeparam>
        /// <param name="sql">The query</param>
        /// <param name="parameters">The parameters in the query</param>
        /// <returns>IEnumerable of type T</returns>
        Task<IEnumerable<T>> LoadDataAsync<T, U>(string sql, U parameters);

        /// <summary>
        /// Load data from the database using sql query and function on the result type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns>IEnumerable of type T</returns>
        Task<IEnumerable<T>> LoadDataAsync<T, U>(string sql, Func<T, U, T> func);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SeveData<T>(string sql, T parameters);
    }
}
