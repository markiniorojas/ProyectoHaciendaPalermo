using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Business.Interfaces;
using Mapster;
using Data.Core;
using Entity.Model;
using Entity.DTO;

namespace Business.Core
{
    /// <summary>
    /// Implementación base sencilla de IServiceBase que proporciona operaciones CRUD genéricas
    /// utilizando un patrón de repositorio y mapeo automático con Mapster.
    /// </summary>
    /// <typeparam name="TDto">Tipo de DTO que hereda de BaseModelDTO.</typeparam>
    /// <typeparam name="TEntity">Tipo de entidad que hereda de BaseModel.</typeparam>
    public class ServiceBase<TDto, TEntity> : IServiceBase<TDto, TEntity>
        where TDto : BaseModelDTO
        where TEntity : BaseModel
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly ILogger _logger;

        protected ServiceBase(IRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public virtual async Task<List<TDto>> GetAllAsync()
          { 
            var entities = await _repository.GetAllAsync();
            return entities.Adapt<List<TDto>>();
        }

        public virtual async Task<List<TDto>> GetAllActiveAsync()
        {
            var entities = await _repository.GetAllAsync();
            var activeEntities = entities.Where(e => e.Active && !e.IsDeleted);
            return activeEntities.Adapt<List<TDto>>();
        }

        public virtual async Task<List<TDto>> GetAllSelectAsync()
        {
            return await GetAllActiveAsync();
        }

        // Cambiado para coincidir con la interfaz (nullable)
        public virtual async Task<TDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity?.Adapt<TDto>();
        }

        public virtual async Task<TDto> GetByNameAsync(string name)
        {
            var entities = await _repository.GetAllAsync();
            var entity = entities.FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return entity?.Adapt<TDto>();
        }

        // Método AddAsync que faltaba (renombrado desde CreateAsync)
        public virtual async Task<TDto> AddAsync(TDto dto)
        {
            var entity = dto.Adapt<TEntity>();
            entity.Active = true;
            entity.IsDeleted = false;

            var created = await _repository.AddAsync(entity);
            return created.Adapt<TDto>();
        }

        // Método Save que implementa la lógica de crear o actualizar
        public virtual async Task<TDto> Save(TDto dto)
        {
            if (dto.Id == 0)
            {
                return await AddAsync(dto);
            }
            else
            {
                return await UpdateAsync(dto);
            }
        }

        // Método SaveDetails que faltaba
        public virtual async Task<TDto[]> SaveDetails(TDto[] details)
        {
            var results = new List<TDto>();

            foreach (var dto in details)
            {
                var result = await Save(dto);
                results.Add(result);
            }

            return results.ToArray();
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = dto.Adapt<TEntity>();
            await _repository.UpdateAsync(entity);
            return dto;
        }

        public virtual async Task UpdateDetailsAsync(TDto[] dtos)
        {
            foreach (var dto in dtos)
            {
                await UpdateAsync(dto);
            }
        }

        public virtual async Task<bool> DeletePermanentAsync(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);

                if (!result)
                {
                    _logger.LogWarning($"No se encontró la entidad {typeof(TEntity).Name} con ID {id} para eliminación física.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar permanentemente {typeof(TEntity).Name} con ID {id}");
                throw; // Relanza la excepción para manejo superior
            }
        }

        public virtual async Task<bool> DeleteLogicalAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                entity.Active = false;
                return await _repository.UpdateAsync(entity);
            }
            return false;
        }

        public virtual async Task<bool> PatchLogicalAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = false;
                entity.Active = true;
                return await _repository.PatchLogicalAsync(id);
            }
            return false;
        }

        public virtual async Task<bool> ToggleActiveAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                entity.Active = !entity.Active;
                return await _repository.UpdateAsync(entity);
            }
            return false;
        }

        public virtual async Task<string> GenerateCodeAsync(string prefix)
        {
            var entities = await _repository.GetAllAsync();
            int count = entities.Count() + 1;
            int currentYear = DateTime.UtcNow.Year;
            return $"{prefix}-{currentYear}-{count.ToString().PadLeft(4, '0')}";
        }
    }
}