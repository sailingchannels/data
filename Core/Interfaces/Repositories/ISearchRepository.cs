using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ISearchRepository
    {
        Task Insert(string query);
    }
}
