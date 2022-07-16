using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.Context;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class UserRepository:BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context)
        {
            Context = context;
        }


        public async Task<User> GetUserBuyer(int id)
        {
            var buyer = await Context.Users.Include(x => x.Buyer).FirstOrDefaultAsync(x => x.Id == id);
            return buyer;
        }
    }
}