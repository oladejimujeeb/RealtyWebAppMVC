using RealtyWebApp.Context;
using RealtyWebApp.Entities;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class PropertyRepository:BaseRepository<Property>,IPropertyRepository
    {
        public PropertyRepository(ApplicationContext context)
        {
            Context =context;
        }
    }
}