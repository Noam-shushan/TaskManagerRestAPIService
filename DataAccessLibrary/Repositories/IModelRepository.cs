using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.DataAccess.Repositories
{
    /// <summary>
    /// A generic repository that runs CRUD operations on the database
    /// </summary>
    /// <typeparam name="TModel">The model</typeparam>
    public interface IModelRepository<TModel> where TModel : class 
    {
        /// <summary>
        /// Get all records of this model
        /// </summary>
        /// <returns>All records of this model</returns>
        Task<IEnumerable<TModel>> GetAllAsync();

        /// <summary>
        /// Get all records the one her the property is equal to some value 
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="propName">The property name</param>
        /// <param name="propValue">The property values</param>
        /// <returns>All records of this model by some condition</returns>
        Task<IEnumerable<TModel>> GetAllAsync<U>(string propName, U propValue);

        /// <summary>
        /// Get all record of the model by sql and dynamic parameters
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="sql">The sql query</param>
        /// <param name="parameters">The parameters to the query</param>
        /// <returns>All records of this model by some condition</returns>
        Task<IEnumerable<TModel>> GetAllByAsync<U>(string sql, U parameters);

        /// <summary>
        /// Get single record of this model
        /// </summary>
        /// <param name="id">The id of the record</param>
        /// <param name="getSimple">Optional flag to get flat object and not full object</param>
        /// <returns>The record match to this id</returns>
        Task<TModel> GetByIdAsync(int id, bool getSimple = false);

        /// <summary>
        /// Insert one record of this model to the database
        /// </summary>
        /// <param name="model">The new record</param>
        /// <returns></returns>
        Task Insert(TModel model);

        /// <summary>
        /// Update one record of this model
        /// </summary>
        /// <param name="model">The record to update</param>
        /// <returns></returns>
        Task Update(TModel model);

        /// <summary>
        /// Delete one record of this model match to the given id
        /// </summary>
        /// <param name="id">The id of the record to delete</param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Save all by sql and dynamic parameters
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task SeveAllBy<U>(string sql, U parameters);

        /// <summary>
        /// Check if some record of this model is exist in the database
        /// </summary>
        /// <param name="id">The id to look for</param>
        /// <returns>true if this record is exist in the database</returns>
        Task<bool> IsExist(int id);
    }
}