using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ILanguageRepository
    {
        Task<List<Language>> GetAll();
    }
}
