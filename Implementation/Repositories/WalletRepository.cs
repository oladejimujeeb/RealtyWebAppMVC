using RealtyWebApp.Context;
using RealtyWebApp.Entities;
using RealtyWebApp.Interface.IRepositories;

namespace RealtyWebApp.Implementation.Repositories
{
    public class WalletRepository:BaseRepository<Wallet>,IWalletRepository
    {
        public WalletRepository(ApplicationContext context)
        {
            Context = context;
        }
    }
}