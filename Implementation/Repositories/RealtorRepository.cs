using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public IList<Realtor> GetAllRealtor()
        {
            var realtor =  Context.Realtors.Include(x => x.User).ToList();
            return realtor;
        }
    }
}