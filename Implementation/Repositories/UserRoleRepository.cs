using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.Context;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class UserRoleRepository:BaseRepository<UserRole>,IUserRoleRepository
    {
        public UserRoleRepository(ApplicationContext context)
        {
            Context = context;
        }

        public async Task<UserRole> GetUserRole(int id)
        {
            var user = await Context.UserRoles.Include(x => x.Role).FirstOrDefaultAsync(x => x.UserId == id);
            return user;
        }
    }
}