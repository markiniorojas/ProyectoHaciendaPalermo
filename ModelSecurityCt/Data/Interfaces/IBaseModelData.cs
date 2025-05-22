using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBaseModelData<T, D> where T: IBaseModel where D: BaseDto
    {
        /// <summary>
        /// Datatable
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<IEnumerable<D>> GetDataTable(QueryFilterDto filters);

        /// <summary>
        /// Obtener
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<D>> GetAllSelect();

        /// <summary>
        /// Obtener por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetById(int id);

        /// <summary>
        /// Obtener por code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<T> GetByCode(string code);

        /// <summary>
        /// Guardar
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> Save(T entity);
        /// <summary>
        /// Guardar Detalles
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T[]> SaveDetails(T[] entity);

        /// <summary>
        /// Actualizar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(T entity);



        /// <summary>
        /// ActualizarPatch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdatePatch(T entity);

        /// <summary>
        /// Eliminar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> Delete(int id);

        /// <summary>
        /// DeleteNoLogic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteNoLogic(int id);
    }
}
