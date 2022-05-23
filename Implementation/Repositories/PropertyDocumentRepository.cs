using RealtyWebApp.Context;
using RealtyWebApp.Entities.File;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class PropertyDocumentRepository:BaseRepository<PropertyDocument>,IPropertyDocument
    {
        public PropertyDocumentRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
    
}