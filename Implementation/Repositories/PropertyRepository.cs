using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.Context;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Models.RequestModel;

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

        public async Task<IList<Property>> SearchWhere(SearchRequest search)
        {
            var property = await Context.Properties.Where(x => x.PropertyType == search.PropertyType &&
                                                         x.State.ToLower() == search.Location.ToLower() &&
                                                         x.Price <= search.Price
                                                         || x.Address.ToLower() == search.KeyWord.ToLower()).ToListAsync();
            return property;
        }

        public IEnumerable<Property> PurchasedProperty(int userId)
        {
            var property = Context.Properties.Include(x=>x.Payment).Where(x=>x.BuyerIdentity==userId).ToList();
            return property;
        }

        public List<Property> AllUnverifiedProperty()
        {
            using (Context)
            {
                var pro = Context.Properties
                    .Where(x => x.BuyerIdentity == 0 && x.IsSold == false && x.VerificationStatus == false)
                    .Include(x => x.Realtor).ThenInclude(x => x.User).ToList();
                return pro;
            }
        }

        public List<Property> AllVerifiedProperty()
        {
            using (Context)
            {
                var property = Context.Properties.Where(x => x.VerificationStatus && !x.IsSold).Include(x=>x.Realtor).
                    ThenInclude(x=>x.User).ToList();
                return property;
            }
        }
    }
}