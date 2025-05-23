using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Core
{
    public  class BaseModelBusiness<T, D> : ABaseModelBusiness<T, D> where T : BaseModel where D : BaseModelDTO
    {
        private readonly IBaseModelData<T, D> _data;
        private readonly IMapper _mapper;

        public BaseModelBusiness(IBaseModelData<T, D> data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }

        public override async Task<int> Delete(int id)
        {
            return await _data.Delete(id);
        }

        public override async Task<int> DeleteNoLogic(int id)
        {
            return await _data.DeleteNoLogic(id);
        }

        public override async Task<List<D>> GetAllSelect()
        {
            IEnumerable<D> lstDto = await _data.GetAllSelect();
            return lstDto.ToList();
        }

        public override async Task<D> GetByCode(string code)
        {
            BaseModel entity = await _data.GetByCode(code);
            BaseModelDTO dto = _mapper.Map<D>(entity);
            return (D)dto;
        }

        public override async Task<D> GetById(int id)
        {
            T entity = await _data.GetById(id);
            D dto = _mapper.Map<D>(entity);
            return dto;
        }

        public override async Task<List<D>> GetDataTable(QueryFilterDto filters)
        {
            return (List<D>)await _data.GetDataTable(filters);
        }

        public override async Task<D> Save(D dto)
        {
            if (dto.Codigo.Length == 3)
            {
                dto.Codigo = await GenerarCodigo(dto.Codigo);
            }
            dto.CreateAt = DateTime.UtcNow.AddHours(-5);
            dto.Active = true;
            BaseModel entity = _mapper.Map<T>(dto);
            entity = await _data.Save((T)entity);
            return _mapper.Map<D>(entity);
        }

        public override async Task Update(D dto)
        {
            BaseModel entity = _mapper.Map<T>(dto);
            entity.UpdateAt = DateTime.UtcNow.AddHours(-5);
            await _data.Update((T)entity);
        }



        public override async Task UpdatePatch(D dto)
        {
            BaseModel entity = _mapper.Map<T>(dto);

            entity.UpdateAt = DateTime.UtcNow.AddHours(-5);
            await _data.UpdatePatch((T)entity);
        }


        public override async Task<D[]> SaveDetails(D[] dtoDetails)
        {
            BaseModel[] details = _mapper.Map<T[]>(dtoDetails);
            await _data.SaveDetails((T[])details);
            return dtoDetails;
        }

        public override async Task UpdateDetails(D[] details)
        {
            foreach (D dto in details)
            {
                BaseModel entity = _mapper.Map<T>(dto);
                entity.UpdateAt = DateTime.UtcNow.AddHours(-5);
                await _data.Update((T)entity);
            }
        }

        public override async Task<string> GenerarCodigo(string prefix)
        {
            IEnumerable<D> entitys = await _data.GetDataTable(new QueryFilterDto { Filter = "" });
            int countEntitys = entitys.Count() + 1;
            return $"{prefix}-{DateTime.UtcNow.AddHours(-5).Year}-{countEntitys.ToString().PadLeft(4, '0')}";
        }
    }
}
