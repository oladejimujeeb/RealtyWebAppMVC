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
    }
}