using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IPropertyRepository _propertyRepository;
        private readonly IVisitationRepository _visitationRepository;
        private readonly IPropertyImage _propertyImage;

        public AdminService(IAdminRepository adminRepository, IRoleRepository roleRepository, IUserRepository userRepository, IPropertyRepository propertyRepository, IVisitationRepository visitationRepository, IPropertyImage propertyImage)
        {
            _adminRepository = adminRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _visitationRepository = visitationRepository;
            _propertyImage = propertyImage;
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

        public async Task<BaseResponseModel<PropertyDto>> GetPropertyById(int id)
        {
            var getProperty = await _propertyRepository.Get(x => x.Id == id);
            if (getProperty == null)
            {
                return new BaseResponseModel<PropertyDto>
                {
                    Status = false,Message = "Not found",
                };
            }

            return new BaseResponseModel<PropertyDto>
            {
                Status = true,
                Data = new PropertyDto()
                {
                    Id = getProperty.Id,
                    Address = getProperty.Address,
                    Bedroom = getProperty.Bedroom,
                    Features = getProperty.Features,
                    Latitude = getProperty.Latitude,
                    Longitude = getProperty.Longitude,
                    Toilet = getProperty.Toilet,
                    BuildingType = getProperty.BuildingType,
                    BuyerId = getProperty.BuyerIdentity,
                    IsSold = getProperty.IsSold,
                    LandArea = getProperty.PlotArea,
                    PropertyPrice = getProperty.Price,
                    RealtorId = getProperty.RealtorId,
                    PropertyType = getProperty.PropertyType,
                    PropertyRegNumber = getProperty.PropertyRegNo,
                    Action = getProperty.Action,
                    Status = getProperty.Status,
                    VerificationStatus = getProperty.VerificationStatus,
                    IsAvailable = getProperty.IsAvailable,
                    LGA = getProperty.LGA,
                    State = getProperty.State,
                    ImagePath =_propertyImage.QueryWhere(y=>y.PropertyRegNo==getProperty.PropertyRegNo).Select(y=>y.DocumentName).ToList() //getProperty.PropertyImages.Select(z=>z.DocumentPath).ToList(),
                },
                Message = "load successfully"
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> AllUnverifiedProperty()
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.BuyerIdentity==0 && x.IsSold==false && x.VerificationStatus==false).
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
                    RegisteredDate = x.RegisteredDate,
                    LGA = x.LGA,
                    State = x.State,
                    //ImagePath = x.PropertyImages.Select(z=>z.DocumentName).ToList(),//Possible Error
                }).OrderBy(x=>x.RegisteredDate).ToList();
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }

        public BaseResponseModel<IEnumerable<VisitationRequestDto>> VisitationRequest()
        {
            var inspectionRequest =
                _visitationRepository.QueryWhere(x => x.RequestDate > DateTime.Now.AddDays(-4)).
                    Select(x=>new VisitationRequestDto
                    {
                        Id = x.Id,
                        BuyerId = x.BuyerId,
                        BuyerName = x.BuyerName,
                        PropertyAddress = x.Address,
                        PropertyId = x.PropertyId,
                        BuyerPhoneNo = x.BuyerTelephone,
                        PropertyType = x.PropertyType,
                        PropertyRegNo = x.PropertyRegNo,
                        RequestDate = x.RequestDate,
                    }).OrderBy(x=>x.RequestDate).ToList();
            if (inspectionRequest.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<VisitationRequestDto>>()
                {
                    Status = false,
                    Message = "No available request"
                };
            }
            return new BaseResponseModel<IEnumerable<VisitationRequestDto>>()
            {
                Status = true,
                Data = inspectionRequest
            };
                    
        }

        public async Task<BaseResponseModel<BaseResponseModel<PropertyDto>>>UpdateRealtorPropertyForSale(int propertyId, UpdatePropertyModel updateProperty)
        {
            var getProperty = await _propertyRepository.Get(x => x.Id == propertyId);
            if (getProperty == null)
            {
                return new BaseResponseModel<BaseResponseModel<PropertyDto>>()
                {
                    Status = false,
                    Message = "Update failed",
                };
            }
            getProperty.Address = updateProperty.Address;
            getProperty.Bedroom = updateProperty.Bedroom;
            getProperty.Features = updateProperty.Features;
            getProperty.Latitude = updateProperty.Latitude;
            getProperty.Longitude = updateProperty.Longitude;
            getProperty.Price = updateProperty.Price;
            getProperty.Status = updateProperty.Status.ToString();
            getProperty.Toilet = updateProperty.Toilet;
            getProperty.BuildingType = updateProperty.BuildingType;
            getProperty.IsAvailable = updateProperty.IsAvailable;
            getProperty.IsSold = updateProperty.IsSold;
            getProperty.VerificationStatus = updateProperty.VerificationStatus;
            getProperty.PlotArea = updateProperty.PlotArea;
            getProperty.PropertyType = updateProperty.PropertyType;
            getProperty.LGA = updateProperty.LGA;
            getProperty.State = updateProperty.State;
            getProperty.Action = updateProperty.Action;
            var upDate = await _propertyRepository.Update(getProperty);
            if (upDate == null)
            {
                return new BaseResponseModel<BaseResponseModel<PropertyDto>>()
                {
                    Status = false,
                    Message = "Update failed",
                };
            }
            return new BaseResponseModel<BaseResponseModel<PropertyDto>>()
            {
                Status = true,
                Message = $"Property {upDate.PropertyRegNo} updated successfully"
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> SearchPropertyByRegNo(SearchRequest searchRequest)
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.PropertyRegNo == searchRequest.PropertyRegNo)
                .Select(x => new PropertyDto()
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
                    IsSold = x.IsSold,
                    LandArea = x.PlotArea,
                    PropertyPrice = x.Price,
                    RealtorId = x.RealtorId,
                    PropertyType = x.PropertyType,
                    PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    LGA = x.LGA,
                    State = x.State,
                    ImagePath = _propertyImage.QueryWhere(y => y.PropertyRegNo == x.PropertyRegNo)
                        .Select(y => y.DocumentName)
                        .ToList() 
                }).ToList();
            if (getProperty.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<PropertyDto>>
                {
                    Status = false,
                    Message = "Not found"
                };
            }

            return new BaseResponseModel<IEnumerable<PropertyDto>>
            {
                Status = true,
                Data = getProperty
            };
        }
    }
}