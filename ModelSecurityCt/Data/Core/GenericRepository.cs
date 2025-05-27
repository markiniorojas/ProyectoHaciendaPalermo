using Entity.context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Core
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseModel
    {
        protected readonly ApplicationDbContext _context;
        protected readonly ILogger _logger;

        public GenericRepository(ApplicationDbContext context,ILogger logger )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>()
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLogicalAsync(int id)
        {
            _logger.LogInformation($"Eliminación lógica de {typeof(TEntity).Name} con ID: {id}");
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity == null) return false;

                entity.IsDeleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en eliminación lógica de {typeof(TEntity).Name} con ID {id}");
                throw;
            }
        }

        public async Task<bool> PatchLogicalAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null) return false;

            var isDeletedProp = entity.GetType().GetProperty("IsDeleted");
            if (isDeletedProp != null)
            {
                isDeletedProp.SetValue(entity, false);
            }

            var activeProp = entity.GetType().GetProperty("Active");
            if (activeProp != null)
            {
                activeProp.SetValue(entity, true);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}