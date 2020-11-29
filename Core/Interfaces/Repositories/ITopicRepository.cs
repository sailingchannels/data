using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ITopicRepository
    {
        Task<List<Topic>> GetAll(string language);
        Task<Topic> Get(string id);
    }
}
