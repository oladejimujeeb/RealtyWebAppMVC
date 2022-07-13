using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IList<Buyer>> GetAllBuyers()
        {
            var buyer =await Context.Buyers.Include(x => x.User).ToListAsync();
            return buyer;
        }
    }
}