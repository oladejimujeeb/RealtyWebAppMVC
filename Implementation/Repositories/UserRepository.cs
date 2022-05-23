using RealtyWebApp.Context;
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
    }
}