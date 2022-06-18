using System;
using System.Collections.Generic;
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
        private readonly IUserRoleRepository _userRoleRepository;

        public RealtorService(IRealtorRepository realtorRepository, IUserRepository userRepository,
            IRoleRepository roleRepository, IWebHostEnvironment webHostEnvironment, IPropertyImage propertyImage, 
            IPropertyDocument propertyDocument, IPropertyRepository propertyRepository,IUserRoleRepository userRoleRepository)
        {
            _realtorRepository = realtorRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _webHostEnvironment = webHostEnvironment;
            _propertyImage = propertyImage;
            _propertyDocument = propertyDocument;
            _propertyRepository = propertyRepository;
            _userRoleRepository = userRoleRepository;
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
            //var fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
            var filePath = Path.Combine(basePath, model.ProfilePicture.FileName);
            var extension = Path.GetExtension(model.ProfilePicture.FileName);
            if (!System.IO.File.Exists(filePath)&& extension==".jpg"|| extension ==".png"|| extension==".jpeg")
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = filePath;
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
            var realtor = await _realtorRepository.Get(x => x.UserId == id);
            //var getRealtorId = await _userRoleRepository.
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
                RealtorId = id,
                IsAvailable = true,
                Status = Status.UnderReview.ToString(),
                PlotArea = model.PlotArea,
                RegisteredDate = DateTime.UtcNow,
                PropertyRegNo = $"PTY{Guid.NewGuid().ToString().Substring(0, 4)}",
                LGA = model.LGA,
                State = model.State,
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
                        UploadedBy = id,
                        //PropertyId = addProperty.Id,
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
                    CreatedOn = DateTime.Now,
                    FileType = file.ContentType,
                    Extension = extension,
                    DocumentName = $"{fileName}{extension}",
                    Description = model.FileDescription,
                    UploadedBy = id,
                    //PropertyId = addProperty.Id,
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
                return new BaseResponseModel<PropertyDto>()
                {
                    Message = "Property Successfully failed",
                    Status = false,
                };
            }
            return new BaseResponseModel<PropertyDto>()
            {
                Message = "Property Successfully Registered",
                Status = true,
                Data =new PropertyDto()
                {
                    ImagePath = registerProperty.PropertyImages.Select(x=>x.DocumentName).ToList()
                }
                
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByRealtorId(int realtorId)
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.RealtorId == realtorId && x.BuyerIdentity==0).
                Select(x=>new PropertyDto()
                {
                    Id = x.Id,
                    Address = x.Address,
                    Bedroom = x.Bedroom,
                    Features = x.Features,
                    IsSold = x.IsSold,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Toilet = x.Toilet,
                    BuildingType = x.BuildingType,
                    LandArea = x.PlotArea,
                    PropertyPrice = x.Price,
                    RealtorId = x.RealtorId,
                    PropertyType = x.PropertyType,
                    PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    LGA = x.LGA,
                    State = x.State,
                    ImagePath = x.PropertyImages.Select(z=>z.DocumentName).ToList(),//Possible Error
                }).ToList();
            if (getProperty.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<PropertyDto>>()
                {
                    Status = false,
                    Message = "No information"
                };
            }
            
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> GetSoldPropertyByRealtor(int realtorId)
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.RealtorId == realtorId && x.IsSold).
                Select(x=>new PropertyDto()
                {
                    Id = x.Id,
                    Address = x.Address,
                    Bedroom = x.Bedroom,
                    Features = x.Features,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Toilet = x.Toilet,
                    BuildingType = x.BuildingType,
                    BuyerId = x.BuyerIdentity,
                    LandArea = x.PlotArea,
                    PropertyPrice = x.Price,
                    RealtorId = x.RealtorId,
                    PropertyType = x.PropertyType,
                    PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    IsSold = x.IsSold,
                    LGA = x.LGA,
                    State = x.State,
                }).ToList();
            if (getProperty.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<PropertyDto>>()
                {
                    Status = false,
                    Message = "No information"
                };
            }
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> GetRealtorApprovedProperty(int id)
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.RealtorId == id && x.VerificationStatus).
                Select(x=>new PropertyDto()
                {
                    Id = x.Id,
                    Address = x.Address,
                    Bedroom = x.Bedroom,
                    Features = x.Features,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Toilet = x.Toilet,
                    BuildingType = x.BuildingType,
                    BuyerId = x.BuyerIdentity,
                    LandArea = x.PlotArea,
                    PropertyPrice = x.Price,
                    RealtorId = x.RealtorId,
                    PropertyType = x.PropertyType,
                    PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    LGA = x.LGA,
                    State = x.State,
                    //ImagePath = x.PropertyImages.Select(z=>z.DocumentName).ToList(),//Possible Error
                }).ToList();
            if (getProperty.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<PropertyDto>>()
                {
                    Status = false,
                    Message = "No information"
                };
            }
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }
    }
}