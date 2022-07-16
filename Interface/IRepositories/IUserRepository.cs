using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities.Identity;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IUserRepository:IBaseRepository<User>
    {
        Task<User> GetUserBuyer(int id);
    }
}