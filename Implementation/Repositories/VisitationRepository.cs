using System;
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
    public class VisitationRepository:BaseRepository<VisitationRequest>, IVisitationRepository
    {
        public VisitationRepository(ApplicationContext context)
        {
            Context = context;
        }

        public IEnumerable<VisitationRequest> Visitation()
        {
            var visit = Context.VisitationRequests.Include(x => x.Property)
                .Where(x => x.RequestDate > DateTime.Now.AddDays(-5)).ToList();
            return visit;
        }

        public async Task<IEnumerable<VisitationRequest>> BuyerInspectedProperty(int buyerId)
        {
            var inspect = await Context.VisitationRequests.Include(x => x.Property)
                .Where(x => x.BuyerId == buyerId && !x.Property.IsSold).ToListAsync();
            return inspect;
        }

        public int CheckIfDateIsAvailable(DateTime date)
        {
                var count =  Context.VisitationRequests.Count(x => x.RequestDate.Date == date.Date);
                return count;
        }
    }
}