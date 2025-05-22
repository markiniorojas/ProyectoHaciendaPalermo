using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBaseModelBusiness<T, D> where T : BaseModel where D : BaseDTO
    {
        /// <summary>
        /// Datatable
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<List<D>> GetDataTable(QueryFilterDto filters);
        /// <summary>
        /// Obtener 
        /// </summary>
        /// <returns></returns>
        Task<List<D>> GetAllSelect();
        /// <summary>
        /// Obtener por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<D> GetById(int id);
        /// <summary>
        /// Obtener por code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<D> GetByCode(string code);
        /// <summary>
        /// Guardar
        /// </summary>
        /// <param name="entityDto"></param>
        /// <returns></returns>
        Task<D> Save(D entityDto);
        /// <summary>
        /// Guardar Detalles
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        Task<D[]> SaveDetails(D[] details);
        /// <summary>
        /// Actualizar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityDto"></param>
        /// <returns></returns>
        Task Update(D entityDto);


        /// <summary>
        /// ActualizarPatch
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityDto"></param>
        /// <returns></returns>
        Task UpdatePatch(D entityDto);

        /// <summary>
        /// Actualizar Detalles
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        Task UpdateDetails(D[] details);
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
