using RealtyWebApp.Context;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class RoleRepository:BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
}