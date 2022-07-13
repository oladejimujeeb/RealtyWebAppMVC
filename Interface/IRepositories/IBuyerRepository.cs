using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.Entities;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IBuyerRepository:IBaseRepository<Buyer>
    {
        Task<IList<Buyer>> GetAllBuyers();
    }
}