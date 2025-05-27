using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;
using Entity.DTO;


namespace Data.Core
{
    public abstract class ABaseModelData<TEntity> : IRepository<TEntity>
        where TEntity : BaseModel
    {

        public abstract Task<List<TEntity>> GetAllAsync();
        public abstract Task<TEntity?> GetByIdAsync(int id);
        public abstract Task<TEntity> AddAsync(TEntity entity);
        public abstract Task<bool> UpdateAsync(TEntity entity);
        public abstract Task<bool> DeleteAsync(int id);
        public abstract Task<bool> DeleteLogicalAsync(int id);
        public abstract Task<bool> PatchLogicalAsync(int id);

       

     

       
    }
}
