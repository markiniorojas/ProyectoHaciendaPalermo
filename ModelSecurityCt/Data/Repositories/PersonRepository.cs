using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Core;
using Data.Interfaces;
using Entity.context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    { 

        public PersonRepository(ApplicationDbContext context, ILogger<PersonRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Person>> ObtenerTodasAsync()
        {
            return await _context.Person.ToListAsync();
        }

        public async Task<List<Person>> ObtenerActivasAsync()
        {
            return await _context.Person.Where(p => !p.IsDeleted).ToListAsync();
        }

    }
}
