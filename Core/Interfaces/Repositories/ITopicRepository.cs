using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ITopicRepository
    {
        Task<IReadOnlyCollection<Topic>> GetAll(string language);
        Task<Topic> Get(string id);
    }
}
