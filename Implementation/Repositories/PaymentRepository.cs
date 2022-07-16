using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.Context;
using RealtyWebApp.Entities;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class PaymentRepository:BaseRepository<Payment>,IPaymentRepository
    {
        public PaymentRepository(ApplicationContext context)
        {
            Context = context;
        }
        public async Task<IList<Payment>> GetAllPayment()
        {
            var payment = await Context.Payments.Where(x=>x.Status==PaymentStatus.Success).Include(x => x.Property).Include(x=>x.Property.Realtor).ToListAsync();
            return payment;
        }
    }
}