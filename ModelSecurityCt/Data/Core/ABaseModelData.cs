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
    public abstract class ABaseModelData<T, D> : IBaseModelData<T, D> where T : BaseModel where D : BaseModelDTO
    {
        public abstract Task<int> Delete(int id);
        public abstract Task<int> DeleteNoLogic(int id);
        public abstract Task<IEnumerable<D>> GetAllSelect();

        public abstract Task<T> GetByCode(string code);

        public abstract Task<T> GetById(int id);

        public abstract Task<IEnumerable<D>> GetDataTable(QueryFilterDto filters);

        public abstract Task<T> Save(T entity);

        public abstract Task<T[]> SaveDetails(T[] entity);

        public abstract Task Update(T entity);

        public abstract Task UpdatePatch(T entity);
    }
}
