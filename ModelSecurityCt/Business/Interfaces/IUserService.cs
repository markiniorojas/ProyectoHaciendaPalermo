using Business.Core;
using Entity.DTO;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService : IServiceBase<UserDTO, User>
    {
        /////<Summary>
        /////Metodo para verificar si es admin
        ///// </Summary>
        /////
        Task<bool> EsAdminAsync(int userId);

    }

}
