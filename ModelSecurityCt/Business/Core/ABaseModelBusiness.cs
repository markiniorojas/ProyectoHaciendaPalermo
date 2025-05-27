using Business.Interfaces;
using Entity.DTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Implementations
{
    /// <summary>
    /// Clase abstracta base para la lógica de negocio que implementa IServiceBase
    /// con operaciones de dominio para entidades que heredan de BaseModel.
    /// </summary>
    /// <typeparam name="TDto">Tipo de DTO que hereda de BaseModelDTO</typeparam>
    /// <typeparam name="TEntity">Tipo de entidad que hereda de BaseModel</typeparam>
    public abstract class ABaseModelBusiness<TDto, TEntity> : IServiceBase<TDto, TEntity>
        where TDto : BaseModelDTO
        where TEntity : BaseModel
    {


        public abstract Task<TDto> AddAsync(TDto dto);
        public abstract Task<TDto> Save(TDto dto);
        public abstract Task<TDto[]> SaveDetails(TDto[] details);
        public abstract Task<List<TDto>> GetAllAsync();
        public abstract Task<List<TDto>> GetAllActiveAsync();
        public abstract Task<List<TDto>> GetAllSelectAsync();
        public abstract Task<TDto> GetByIdAsync(int id);
        public abstract Task<TDto> GetByNameAsync(string name);


        public abstract Task<TDto> CreateAsync(TDto dto);
        public abstract Task<TDto> UpdateAsync(TDto dto);
        public abstract Task UpdateDetailsAsync(TDto[] dtos);

       
        public abstract Task<bool> DeletePermanentAsync(int id);
        public abstract Task<bool> DeleteLogicalAsync(int id);
        public abstract Task<bool> PatchLogicalAsync(int id);
        public abstract Task<bool> ToggleActiveAsync(int id);

        public abstract Task<string> GenerateCodeAsync(string prefix);


        protected virtual async Task<List<string>> ValidateForCreateAsync(TDto dto)
        {
            var errors = new List<string>();

            if (dto == null)
            {
                errors.Add("Los datos de la entidad son requeridos");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                errors.Add("El nombre es requerido");
            }

            return errors;
        }

        protected virtual async Task<List<string>> ValidateForUpdateAsync(TDto dto)
        {
            var errors = new List<string>();

            if (dto == null)
            {
                errors.Add("Los datos de la entidad son requeridos");
                return errors;
            }

            if (dto.Id <= 0)
            {
                errors.Add("ID de entidad inválido");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                errors.Add("El nombre es requerido");
            }

            return errors;
        }

        protected virtual async Task ApplyBusinessRulesForCreateAsync(TDto dto)
        {
            if (dto != null)
            {
                dto.Active = true;
                dto.IsDeleted = false;
            }
            await Task.CompletedTask;
        }

        protected virtual async Task ApplyBusinessRulesForUpdateAsync(TDto dto)
        {
            await Task.CompletedTask;
        }

    }
}