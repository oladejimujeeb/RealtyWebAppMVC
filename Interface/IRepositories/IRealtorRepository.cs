using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.Entities;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IRealtorRepository:IBaseRepository<Realtor>
    {
        IList<Realtor>GetAllRealtor();
    }
}