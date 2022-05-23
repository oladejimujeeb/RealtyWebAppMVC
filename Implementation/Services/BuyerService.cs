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
    public class BuyerService:IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public BuyerService(IBuyerRepository buyerRepository, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _buyerRepository = buyerRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }
        public async Task<BaseResponseModel<BuyerDto>> RegisterBuyer(BuyerRequestModel model)
        {
            var checkMail = await _userRepository.Exists(x => x.Email == model.Email);
            if (checkMail)
            {
                return new BaseResponseModel<BuyerDto>()
                {
                    Message = "Email Already Exist",
                    Status = false
                };
            }
            var role = RoleConstant.Buyer.ToString();
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
            if (!System.IO.File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = fileName;
                await _userRepository.Add(user);
            }

            var buyer = new Buyer()
            {
                Address = model.Address,
                User = user,
                UserId = user.Id,
                RegId = $"CLT/{Guid.NewGuid().ToString().Substring(0, 4)}",
            };
            var addBuyer = await _buyerRepository.Add(buyer);
            if (addBuyer != buyer)//potential error
            {
                return new BaseResponseModel<BuyerDto>()
                {
                    Status = false,
                    Message = "Registration  failed"
                };
            }
            return new BaseResponseModel<BuyerDto>()
            {
                Status = false,
                Message = "Registration  failed",
                Data = new BuyerDto()
                {
                    RegNo = addBuyer.RegId
                }
            };
        }
    }
}