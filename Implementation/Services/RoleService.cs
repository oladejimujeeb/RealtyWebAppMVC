using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;

namespace RealtyWebApp.Implementation.Services
{
    public class RoleService:IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<BaseResponseModel<RoleDto>> CreateRole(string roleName)
        {
            var addRole = new Role()
            {
                RoleName = roleName
            };
            var role = await _roleRepository.Add(addRole);
            return new BaseResponseModel<RoleDto>()
            {
                Status = true,
                Message = "Role Add Successfully"
            };
        }

        public async Task<BaseResponseModel<RoleDto>> GetRole(string roleName)
        {
            var getRole = await _roleRepository.Get(x => x.RoleName == roleName);
            if (getRole == null)
            {
                return new BaseResponseModel<RoleDto>
                {
                    Status = false
                };
            }

            return new BaseResponseModel<RoleDto>()
            {
                Status = true,
                Data = new RoleDto()
                {
                    Id = getRole.Id,
                    RoleName = getRole.RoleName
                }
            };
        }
    }
}