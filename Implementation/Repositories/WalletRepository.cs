using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public Wallet GetWalletDetails(int realtorId)
        {
            var wallet = Context.Wallets.Where(x => x.RealtorId == realtorId).Include(x => x.Realtor)
                .Include(x => x.Realtor.User).SingleOrDefault();
            return wallet;
        }
    }
}