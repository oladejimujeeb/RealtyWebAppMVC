using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.Entities;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IPaymentRepository:IBaseRepository<Payment>
    {
       Task<IList<Payment>>GetAllPayment();
    }
}