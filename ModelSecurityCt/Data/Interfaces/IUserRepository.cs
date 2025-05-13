using System.Threading.Tasks;
using Data.Core;
using Entity.DTO;
using Entity.Model;

namespace Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> validacionUser(LoginDTO dto);
    }
}