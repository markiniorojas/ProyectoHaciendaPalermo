using Business.Core;
using Business.Interfaces;
using Data.Interfaces;
using Data.Repositories;
using Entity.DTO;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class FormService : ServiceBase<FormDTO, Form>, IFormService
    {
        public FormService(IFormRepository repository, ILogger<FormService> logger)
            : base(repository, logger) { }
    }
}
