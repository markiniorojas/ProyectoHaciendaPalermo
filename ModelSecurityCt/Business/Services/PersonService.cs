using Business.Core;
using Business.Interfaces;
using Data.Interfaces;
using Entity.DTO;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Business.Services
{
    public class PersonService : ServiceBase<PersonDTO, Person>
    {

        private readonly IPersonRepository _personRepository;
        private readonly IUserService _userService;

        public PersonService(IPersonRepository repository, ILogger<PersonService> logger, IUserService userService)
            : base(repository, logger) {

            _personRepository = repository;
            _userService = userService;
        }

        public async Task<List<Person>> ObtenerPersonasSegunRolAsync(int userId)
        {
            var esAdmin = await _userService.EsAdminAsync(userId);

            if (esAdmin)
            {
                return await _personRepository.ObtenerTodasAsync();
            }
            else
            {
                return await _personRepository.ObtenerActivasAsync();
            }
        }
    }

}
