using RealtyWebApp.Context;
using RealtyWebApp.Entities;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class VisitationRepository:BaseRepository<VisitationRequest>, IVisitationRepository
    {
        public VisitationRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
}