using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Model;
using Entity.DTO;

namespace Business.Interfaces
{
    /// <summary>
    /// Interfaz base unificada para servicios de negocio que define operaciones CRUD genéricas
    /// y operaciones de negocio avanzadas para entidades que heredan de BaseModel.
    /// </summary>
    /// <typeparam name="TDto">Tipo de objeto de transferencia de datos (DTO) que hereda de BaseModelDTO.</typeparam>
    /// <typeparam name="TEntity">Tipo de entidad del dominio que hereda de BaseModel.</typeparam>
    public interface IServiceBase<TDto, TEntity>
        where TDto : BaseModelDTO
        where TEntity : BaseModel
    {


        /// <summary>
        /// Obtiene todas las entidades.
        /// </summary>
        /// <returns>Lista de DTOs.</returns>
        Task<List<TDto>> GetAllAsync();

        /// <summary>
        /// Obtiene una entidad por su ID.
        /// </summary>
        /// <param name="id">ID de la entidad.</param>
        /// <returns>DTO de la entidad encontrada.</returns>
        Task<TDto?> GetByIdAsync(int id);

        /// <summary>
        /// Crea una nueva entidad.
        /// </summary>
        /// <param name="dto">DTO con los datos de la entidad a crear.</param>
        /// <returns>DTO de la entidad creada.</returns>
        Task<TDto> AddAsync(TDto dto);

        /// <summary>
        /// Obtiene todas las entidades activas (Active = true y IsDeleted = false).
        /// </summary>
        /// <returns>Lista de DTOs activos.</returns>
        Task<List<TDto>> GetAllActiveAsync();

        /// <summary>
        /// Obtiene todas las entidades activas para listas de selección.
        /// </summary>
        /// <returns>Lista de DTOs simplificados.</returns>
        Task<List<TDto>> GetAllSelectAsync();

        

        /// <summary>
        /// Obtiene una entidad por su nombre.
        /// </summary>
        /// <param name="name">Nombre de la entidad.</param>
        /// <returns>DTO de la entidad encontrada.</returns>
        Task<TDto> GetByNameAsync(string name);

        /// <summary>
        /// Guardar
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<TDto> Save(TDto dto);

        /// <summary>
        /// Guardar Detalles
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        Task<TDto[]> SaveDetails(TDto[] details);


        /// <summary>
        /// Actualiza una entidad existente.
        /// </summary>
        /// <param name="dto">DTO con los datos actualizados.</param>
        /// <returns>DTO de la entidad actualizada.</returns>
        Task<TDto> UpdateAsync(TDto dto);


        /// <summary>
        /// Actualiza múltiples entidades relacionadas.
        /// </summary>
        /// <param name="dtos">Array de DTOs a actualizar.</param>
        Task UpdateDetailsAsync(TDto[] dtos);


        /// <summary>
        /// Realiza una eliminación lógica, útil para mantener historial o trazabilidad.
        /// La entidad debe implementar una propiedad llamada 'IsDeleted' de tipo boolean.
        /// </summary>
        /// <param name="id">Identificador de la entidad a marcar como eliminada.</param>
        /// <returns>True si se marcó como eliminada, False si no fue posible.</returns>
        Task<bool> DeletePermanentAsync(int id);


        /// <summary>
        /// Realiza una eliminación lógica, útil para mantener historial o trazabilidad.
        /// La entidad debe implementar una propiedad llamada 'IsDeleted' de tipo boolean.
        /// </summary>
        /// <param name="id">Identificador de la entidad a marcar como eliminada.</param>
        /// <returns>True si se marcó como eliminada, False si no fue posible.</returns>
        Task<bool> DeleteLogicalAsync(int id);

        /// <summary>
        /// Restaura una entidad eliminada lógicamente.
        /// </summary>
        /// <param name="id">ID de la entidad a restaurar.</param>
        /// <returns>True si se restauró correctamente.</returns>
        Task<bool> PatchLogicalAsync(int id);


        /// <summary>
        /// Cambia el estado activo de una entidad (Active = !Active).
        /// </summary>
        /// <param name="id">ID de la entidad.</param>
        /// <returns>True si se cambió correctamente.</returns>
        Task<bool> ToggleActiveAsync(int id);
     

       
        /// <summary>
        /// Genera un código único para la entidad.
        /// </summary>
        /// <param name="prefix">Prefijo del código.</param>
        /// <returns>Código generado.</returns>
        Task<string> GenerateCodeAsync(string prefix);

    }
}