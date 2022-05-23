using RealtyWebApp.Context;
using RealtyWebApp.Entities.File;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class PropertyImageRepository:BaseRepository<PropertyImage>,IPropertyImage
    {
        public PropertyImageRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
    
}