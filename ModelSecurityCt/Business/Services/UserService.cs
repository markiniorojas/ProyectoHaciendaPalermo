using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Entity.Model;
using Entity.DTO;
using Data.Core;
using Business.Interfaces;
using Business.Core;
using Data.Interfaces;

namespace Business.Services
{
    public class UserService : ServiceBase<UserDTO, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository,  ILogger<UserService> logger) 
            : base(userRepository, logger)
        { 
            _userRepository = userRepository;
            _logger = logger;
        }
    }
}