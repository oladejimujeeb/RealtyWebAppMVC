using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.Entities;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IVisitationRepository:IBaseRepository<VisitationRequest>
    {
        IEnumerable<VisitationRequest> Visitation();
    }
}