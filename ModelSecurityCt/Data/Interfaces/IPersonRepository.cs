using Data.Core;
using Entity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de personas.
    /// Extiende de IGenericRepository<Person> para heredar todos los métodos CRUD básicos,
    /// y define métodos adicionales específicos para la entidad Person.
    /// </summary>
    public interface IPersonRepository : IRepository<Person>
    {
        // Aquí se definen métodos específicos para Person que no están en IGenericRepository<T>

    

        // Puedes agregar más métodos específicos según necesites
        // Por ejemplo:
        // Task<Person> GetByDocumentAsync(string document);
        // Task<IEnumerable<Person>> GetByAgeRangeAsync(int minAge, int maxAge);
    }
}