using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities.Identity;

namespace RealtyWebApp.Interface.IServices
{
    public interface IRoleService
    {
        Task<BaseResponseModel<RoleDto>> CreateRole(string roleName);
        Task<BaseResponseModel<RoleDto>> GetRole(string roleName);
    }
}