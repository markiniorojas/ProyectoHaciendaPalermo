using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Core
{
    public interface IServiceBase<TDTO, TEntity>
    {
        /// <summary>
        ///Obtienes todos los formatos en DTO
        ///</summary>>
        Task<IEnumerable<TDTO>> GetAllAsync();

        /// <summary>
        /// Obtienes un formato por id en DTO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TDTO> GetByIdAsync(int id);

        /// <summary>
        /// Crea un nuevo formato en DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TDTO> CreateAsync(TDTO dto);

        /// <summary>
        /// Actualiza un formato existente en DTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TDTO> UpdateAsync(TDTO dto);

        /// <summary>
        /// Elimina un formato existente de forma permanente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePermanentAsync(int id);

        /// <summary>
        /// Elimina un formato existente de forma lógica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteLogicalAsync(int id);

        /// <summary>
        /// Restablece un formato eliminado de forma lógica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> PatchLogicalAsync(int id);
    }
}
