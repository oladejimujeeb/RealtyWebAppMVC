using RealtyWebApp.Context;
using RealtyWebApp.Entities;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class BuyerRepository:BaseRepository<Buyer>,IBuyerRepository
    {
        public BuyerRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
}