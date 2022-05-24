using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;
using RealtyWebApp.Entities.File;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Entities.Identity.Enum;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Implementation.Services
{
    public class RealtorService:IRealtorService
    {
        private readonly IRealtorRepository _realtorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPropertyImage _propertyImage;
        private readonly IPropertyDocument _propertyDocument;
        private readonly IPropertyRepository _propertyRepository;

        public RealtorService(IRealtorRepository realtorRepository, IUserRepository userRepository, IRoleRepository roleRepository, IWebHostEnvironment webHostEnvironment, IPropertyImage propertyImage, IPropertyDocument propertyDocument, IPropertyRepository propertyRepository)
        {
            _realtorRepository = realtorRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _webHostEnvironment = webHostEnvironment;
            _propertyImage = propertyImage;
            _propertyDocument = propertyDocument;
            _propertyRepository = propertyRepository;
        }

        public async Task<BaseResponseModel<RealtorDto>> RegisterRealtor(RealtorRequestModel model)
        {
            var checkMail = await _userRepository.Exists(x => x.Email.ToLower() == model.Email.ToLower());
            if (checkMail)
            {
                return new BaseResponseModel<RealtorDto>()
                {
                    Message = "Email Already Exist",
                    Status = false
                };
            }

            var role = RoleConstant.Realtor.ToString();
            var getRole = await _roleRepository.Get(x => x.RoleName == role);
            if (getRole == null)
            {
                return new BaseResponseModel<RealtorDto>()
                {
                    Status = false,
                    Message = "Failed"
                };
            }
            
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

            var realtor = new Realtor()
            {
                Address = model.Address,
                BusinessName = model.BusinessName,
                CacRegistrationNumber = model.CacNumber,
                AgentId = $"REG/{Guid.NewGuid().ToString().Substring(0, 4)}",
                UserId = user.Id,
                User = user,
            };
            var addRealtor = await _realtorRepository.Add(realtor);
            return new BaseResponseModel<RealtorDto>
            {
                Status = true,
                Message = "Account Created Successfully",
                Data = new RealtorDto()
                {
                    AgentId = addRealtor.AgentId
                }
                
            };
        }

        public async Task<BaseResponseModel<RealtorDto>> UpdateRealtorInfo(RealtorUpdateRequest model, int id)
        {
            var userInfo = await _userRepository.Get(x => x.Id == id);
            userInfo.FirstName = model.FirstName;
            userInfo.Password = model.Password;
            userInfo.FirstName = model.LastName;
            userInfo.PhoneNumber = model.PhoneNumber;
            var realtor = await _realtorRepository.Get(x => x.Id == userInfo.Realtor.Id);
            if (realtor == null)
            {
                return new BaseResponseModel<RealtorDto>(){Message = "Failed to update", Status = false};
            }
            realtor.Address = model.Address;
            realtor.BusinessName = model.BusinessName;
            realtor.CacRegistrationNumber = model.CacNumber;
            var u = await _userRepository.Update(userInfo);
            var a = await _realtorRepository.Update(realtor);
            return new BaseResponseModel<RealtorDto>()
            {
                Status = true,
                Message = "Update Successfully"
            };

        }

        public async Task<BaseResponseModel<PropertyDto>> AddProperty(PropertyRequestModel model, int id)
        {
            var getRealtor = await _realtorRepository.Get(x => x.Id == id);
            if (getRealtor==null)
            {
                return new BaseResponseModel<PropertyDto>()
                {
                    Status = false,
                    Message = "Failed"
                };
            }
            var addProperty = new Property()
            {
                VerificationStatus = false,
                Address = model.Address,
                Bedroom = model.Bedroom,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Features = model.Features,
                Toilet = model.Toilet,
                Price = model.Price,
                BuildingType = model.BuildingType,
                PropertyType = model.PropertyType,
                RealtorId = getRealtor.Id,//Possible modification here to realtorId
                IsAvailable = true,
                Status = Status.UnderReview.ToString(),
                PlotArea = model.PlotArea,
                PropertyRegNo = $"PTY{Guid.NewGuid().ToString().Substring(0, 4)}",
            };
            //AddingProperty Image
            foreach (var image in model.Images)
            {
                var basePath = Path.Combine(_webHostEnvironment.WebRootPath, "PropertyImages");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(image.FileName);
                var filePath = Path.Combine(basePath, image.FileName);
                var extension = Path.GetExtension(image.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    var imageModel = new PropertyImage()
                    {
                        CreatedOn = DateTime.UtcNow,
                        FileType = image.ContentType,
                        Extension = extension,
                        DocumentName = fileName,
                        Description = model.FileDescription,
                        DocumentPath = filePath,
                        UploadedBy = getRealtor.Id,
                        PropertyRegNo = addProperty.PropertyRegNo
                    };
                    addProperty.PropertyImages.Add(imageModel);
                    await _propertyImage.Add(imageModel);
                }
            }
            //Adding PropertyDocument
            foreach (var file in model.Files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new PropertyDocument()
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    DocumentName = fileName,
                    Description = model.FileDescription,
                    UploadedBy = getRealtor.Id,
                    PropertyRegNo = addProperty.PropertyRegNo
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                addProperty.PropertyDocuments.Add(fileModel);
                await _propertyDocument.Add(fileModel);
            }

            var registerProperty = await _propertyRepository.Add(addProperty);
            if (registerProperty != addProperty)
            {
                
            }
            return new BaseResponseModel<PropertyDto>()
            {
                Message = "Property Successfully Registered",
                Status = true,
                Data = new PropertyDto()
                {
                    PropertyRegNumber = registerProperty.PropertyRegNo
                }
                
            };
        }
    }
}