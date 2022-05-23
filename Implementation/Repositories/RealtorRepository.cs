using RealtyWebApp.Context;
using RealtyWebApp.Entities;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class RealtorRepository:BaseRepository<Realtor>,IRealtorRepository
    {
        public RealtorRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
}