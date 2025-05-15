using Data.Core;
using Entity.DTO;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {

        Task<User> validacionUser(LoginDTO dto);

        ///<Summary>
        ///Metodo para el auth con google
        /// </Summary>
        ///
        Task<User?> getByEmail(string email);
    }
}