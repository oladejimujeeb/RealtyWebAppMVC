using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.DTOs;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Implementation.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<BaseResponseModel<UserDto>> GetUser(LoginModel model)
        {
            var user = await _userRepository.Get(x => x.Email == model.Email);
            if (user == null)
            {
                return new BaseResponseModel<UserDto>()
                {
                    Status = false,
                    Message = "Invalid Credential"
                };
            }
            var userVerify = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if(userVerify == false)
            {
                return new BaseResponseModel<UserDto>()
                {
                    Status = false,
                    Message = "Invalid Credential"
                };
            }

            var userRole = await _userRoleRepository.GetUserRole(user.Id);
            
            return new BaseResponseModel<UserDto>()
            {
                Message = $"Hi {user.FirstName}",
                Status = true,
                Data = new UserDto()
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    RoleName = userRole.Role.RoleName,
                    UserName = $"{user.FirstName} {user.LastName}"
                }
            };
        }

        public Task<BaseResponseModel<UserDto>> GetUserById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}