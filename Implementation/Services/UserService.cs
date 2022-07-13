using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.DTOs;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.MailFolder.EmailService;
using RealtyWebApp.MailFolder.MailEntities;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Implementation.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRealtorRepository _realtorRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IMailService _mailService;

        public UserService(IUserRepository userRepository, IRealtorRepository realtorRepository, IUserRoleRepository userRoleRepository, IBuyerRepository buyerRepository, IAdminRepository adminRepository,IMailService mailService)
        {
            _userRepository = userRepository;
            _realtorRepository = realtorRepository;
            _userRoleRepository = userRoleRepository;
            _buyerRepository = buyerRepository;
            _adminRepository = adminRepository;
            _mailService = mailService;
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
            /*var realtor = await _realtorRepository.Get(x => x.UserId == user.Id);
            var buyer = await _buyerRepository.Get(x => x.UserId == user.Id);*/
            /*var mail = new WelcomeMessage()
            {
                Email = "oladejimujib@gmail.com",
                Id = "mujib007",
                FullName = "Oladeji mujib"
            };
            try
            {
                await _mailService.WelcomeMail(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }*/
           
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
                    AppUserId = await GetRoleId(userRole.Role.RoleName,user.Id),
                    RoleName = userRole.Role.RoleName,
                    UserName = $"{user.FirstName} {user.LastName}",
                    ProfilePicture = user.ProfilePicture
                }
            };
        }
        private async Task<int> GetRoleId(string role, int userId)
        {
            if (role == "Realtor")
            {
                var a = await _realtorRepository.Get(x => x.UserId == userId);
                return a.Id;
            }
            else if (role=="Buyer")
            {
                var a = await _buyerRepository.Get(x => x.UserId == userId);
                return a.Id;
            }
            var r = await _adminRepository.Get(x => x.UserId == userId);
            return r.Id;
          
        }
        public Task<BaseResponseModel<UserDto>> GetUserById(int id)
        {
            throw new System.NotImplementedException();
        }

        public BaseResponseModel<UserDto> GetAllRealtor()
        {
            throw new System.NotImplementedException();
        }
    }
}