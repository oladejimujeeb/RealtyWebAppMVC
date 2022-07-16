using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.Context;
using RealtyWebApp.DTOs;
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

        public async Task<Property> GetWhere(string propertyId)
        {
            var property = await Context.Properties.Where(x=>x.IsSold==false && x.PropertyRegNo==propertyId).FirstOrDefaultAsync();
            return property;
        }
    }
}