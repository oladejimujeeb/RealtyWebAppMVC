using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities.Identity;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IUserRoleRepository:IBaseRepository<UserRole>
    {
        Task<UserRole> GetUserRole(int id);
    }
}