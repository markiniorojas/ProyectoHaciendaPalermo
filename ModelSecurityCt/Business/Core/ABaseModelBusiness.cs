using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core
{
    public abstract class ABaseModelBusiness<T, D>: IBaseModelBusiness<T, D> where T : BaseModel where D : BaseDto
    {
        public abstract Task<int> Delete(int id);
        public abstract Task<int> DeleteNoLogic(int id);
        public abstract Task<List<D>> GetAllSelect();

        public abstract Task<D> GetByCode(string code);

        public abstract Task<D> GetById(int id);

        public abstract Task<List<D>> GetDataTable(QueryFilterDto filters);

        public abstract Task<D> Save(D entityDto);

        public abstract Task<D[]> SaveDetails(D[] entityDto);

        public abstract Task Update(D entityDto);

        public abstract Task UpdatePatch(D entityDto);


        public abstract Task UpdateDetails(D[] details);

        public abstract Task<string> GenerarCodigo(string prefix);
    }
}
