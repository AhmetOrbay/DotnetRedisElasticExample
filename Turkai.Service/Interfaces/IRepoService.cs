using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turkai.Service.Interfaces
{
    public interface IRepoService <T,Tout>
    {
        Task Added(T entity);
        Task<Tout> GetById(long Id);
        Task<List<Tout>> GetAll();
    }
}
