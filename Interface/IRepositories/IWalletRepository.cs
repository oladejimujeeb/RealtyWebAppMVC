using RealtyWebApp.Entities;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IWalletRepository:IBaseRepository<Wallet>
    {
        Wallet GetWalletDetails(int realtorId);
    }
}