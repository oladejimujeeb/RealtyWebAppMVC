using RealtyWebApp.Context;
using RealtyWebApp.Entities;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class AdminRepository:BaseRepository<Admin>,IAdminRepository
    {
        public AdminRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
}