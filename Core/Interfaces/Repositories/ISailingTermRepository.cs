using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISailingTermRepository
    {
        Task<List<SailingTerm>> GetAll();
    }
}
