using System;
using System.IO;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Implementation.Services
{
    public class AdminService:IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public AdminService(IAdminRepository adminRepository, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _adminRepository = adminRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
        public async Task<BaseResponseModel<AdminDto>> RegisterAdmin(AdminRequestModel model)
        {
            var checkMail = await _userRepository.Exists(x => x.Email == model.Email);
            if (checkMail)
            {
                return new BaseResponseModel<AdminDto>()
                {
                    Message = "Email Already Exist",
                    Status = false
                };
            }
            var role = RoleConstant.Administrator.ToString();
            var getRole = await _roleRepository.Get(x => x.RoleName == role);
            
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
            };
            var userRole = new UserRole
            {
                User = user,
                UserId = user.Id,
                RoleId = getRole.Id,
            };
            user.UserRoles.Add(userRole);
            
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\ProfilePictures\\");
            bool basePathExists = System.IO.Directory.Exists(basePath);
            if (!basePathExists)
            {
                Directory.CreateDirectory(basePath);
            }
            var fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
            var filePath = Path.Combine(basePath, model.ProfilePicture.FileName);
            var extension = Path.GetExtension(model.ProfilePicture.FileName);
            if (!System.IO.File.Exists(filePath)&& extension=="jpg"|| extension =="png"|| extension=="jpeg")
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = fileName;
                await _userRepository.Add(user);
            }

            var admin = new Admin
            {
                Address = model.Address,
                RegId = $"AD/{Guid.NewGuid().ToString().Substring(0, 4)}",
                User = user,
                UserId = user.Id,
            };
            var addAdmin = await _adminRepository.Add(admin);
            return new BaseResponseModel<AdminDto>()
            {
                Status = true,
                Message = "Created Successfully",
                Data = new AdminDto()
                {
                    RegNo = addAdmin.RegId
                }
            };
        }
    }
}