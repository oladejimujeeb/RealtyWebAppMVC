using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;
using RealtyWebApp.Interface.IServices.IPropertyMethod;
using RealtyWebApp.MailFolder.EmailService;
using RealtyWebApp.MailFolder.MailEntities;

namespace RealtyWebApp.Implementation.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IVisitationRepository _visitationRepository;
        private readonly IPropertyImage _propertyImage;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRealtorRepository _realtorRepository;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IPropertyServiceMethod _propertyServiceMethod;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMailService _mailService;

        public AdminService(IAdminRepository adminRepository, IRoleRepository roleRepository,
            IUserRepository userRepository, IPropertyRepository propertyRepository,
            IVisitationRepository visitationRepository, IPropertyImage propertyImage,
            IPropertyServiceMethod propertyServiceMethod, IPaymentRepository paymentRepository,
            IWebHostEnvironment webHostEnvironment,IRealtorRepository realtorRepository, IBuyerRepository buyerRepository,IMailService mailService)
        {
            _adminRepository = adminRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _visitationRepository = visitationRepository;
            _propertyImage = propertyImage;
            _webHostEnvironment = webHostEnvironment;
            _realtorRepository = realtorRepository;
            _buyerRepository = buyerRepository;
            _propertyServiceMethod = propertyServiceMethod;
            _paymentRepository = paymentRepository;
            _mailService = mailService;
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

            var basePath = Path.Combine(_webHostEnvironment.WebRootPath, "\\ProfilePictures\\");
            bool basePathExists = System.IO.Directory.Exists(basePath);
            if (!basePathExists)
            {
                Directory.CreateDirectory(basePath);
            }

            var fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
            var filePath = Path.Combine(basePath, model.ProfilePicture.FileName);
            var extension = Path.GetExtension(model.ProfilePicture.FileName);
            if (!System.IO.File.Exists(filePath) && extension == ".jpg" || extension == ".png" || extension == ".jpeg")
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = fileName + extension;
                await _userRepository.Add(user);
                var admin = new Admin
                {
                    Address = model.Address,
                    RegId = $"AD/{Guid.NewGuid().ToString().Substring(0, 4)}",
                    User = user,
                    UserId = user.Id,
                };
                var addAdmin = await _adminRepository.Add(admin);
                var sendMail = new WelcomeMessage()
                {
                    Email = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Id = admin.RegId
                };
                //send mail
                await _mailService.WelcomeMail(sendMail);
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

            return new BaseResponseModel<AdminDto>()
            {
                Message = "failed Upload only image as profile picture",
                Status = false
            };

        }

        public async Task<BaseResponseModel<PropertyDto>> GetPropertyById(int id)
        {
            var property = await _propertyServiceMethod.GetPropertyById(id);
            if (property.Status)
            {
                return new BaseResponseModel<PropertyDto>()
                {
                    Message = property.Message,
                    Status = property.Status,
                    Data = property.Data
                };
            }

            return new BaseResponseModel<PropertyDto>()
            {
                Data = property.Data,
                Status = property.Status,
                Message = property.Message
            };
        }
        public async Task<BaseResponseModel<PropertyDocumentDto>> DownloadPropertyDocument(int documentId)
        {
            var document = await _propertyServiceMethod.DownloadPropertyDocument(documentId);
            if (document.Status)
            {
                return new BaseResponseModel<PropertyDocumentDto>()
                {
                    Status = document.Status,
                    Message = document.Message,
                    Data = document.Data
                };
            }

            return new BaseResponseModel<PropertyDocumentDto>()
            {
                Data = document.Data,
                Status = document.Status,
                Message = document.Message
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> AllUnverifiedProperty()
        {
            var getProperty = _propertyRepository
                .AllUnverifiedProperty().Select(x =>
                    new PropertyDto()
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
                        AgentName =$"{x.Realtor.User.FirstName} {x.Realtor.User.LastName}",
                        AgentId = x.Realtor.AgentId
                        //ImagePath = _propertyImage.QueryWhere(y=>y.PropertyRegNo==x.PropertyRegNo).Select(y=>y.DocumentName).ToList()
                    }).OrderByDescending(x => x.RegisteredDate).ToList();
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data = getProperty
            };
        }

        public BaseResponseModel<IEnumerable<VisitationRequestDto>> VisitationRequest()
        {

            var inspectionRequest =
                _visitationRepository.Visitation().Select(x => new VisitationRequestDto
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
                    Mail = x.BuyerEmail,
                    PropertyPrice = x.Property.Price,
                }).OrderByDescending(x => x.RequestDate).ToList();
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

        public async Task<BaseResponseModel<BaseResponseModel<PropertyDto>>> UpdateRealtorPropertyForSale(
            int propertyId, UpdatePropertyModel updateProperty)
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

            if (updateProperty.Status == "Approved")
            {
                getProperty.IsAvailable = true;
                getProperty.VerificationStatus = true;
            }

            getProperty.Address = updateProperty.Address;
            getProperty.Bedroom = updateProperty.Bedroom;
            getProperty.Features = updateProperty.Features;
            getProperty.Price = updateProperty.Price;
            getProperty.Status = updateProperty.Status;
            getProperty.Toilet = updateProperty.Toilet;
            getProperty.BuildingType = updateProperty.BuildingType;
            getProperty.PlotArea = updateProperty.PlotArea;
            getProperty.PropertyType = updateProperty.PropertyType;
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

        public BaseResponseModel<IEnumerable<PropertyDto>> AllVerifiedProperty()
        {
            var getProperty = _propertyRepository.AllVerifiedProperty().Select(x => new PropertyDto()
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
                RegisteredDate = x.RegisteredDate,
                AgentName = $"{x.Realtor.User.LastName} {x.Realtor.User.FirstName}",
                AgentId = x.Realtor.AgentId,
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
                Data = getProperty
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> AllSoldProperty()
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.IsSold).Select(x => new PropertyDto()
            {
                Id = x.Id,
                Address = x.Address,
                Features = x.Features,
                Toilet = x.Toilet,
                BuildingType = x.BuildingType,
                BuyerId = x.BuyerIdentity,
                LandArea = x.PlotArea,
                PropertyPrice = x.Price,
                RealtorId = x.RealtorId,
                PropertyType = x.PropertyType,
                PropertyRegNumber = x.PropertyRegNo,
                PropertyRegNo = x.PropertyRegNo,
                RegisteredDate = x.RegisteredDate,
                Status = x.Status,
                //SoldDate = 
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
                Data = getProperty
            };
        }

        public BaseResponseModel<IEnumerable<RealtorDto>> AllRealtors()
        {
            var realtor = _realtorRepository.GetAllRealtor().Select(x => new RealtorDto()
            {
                Id = x.Id,
                AgentId = x.AgentId,
                Mail = x.User.Email,
                PhoneNumber = x.User.PhoneNumber,
                FName = $"{x.User.LastName} {x.User.FirstName}",
                BusinessName = x.BusinessName,
                CacNo = x.CacRegistrationNumber,
                Address = x.Address,
                ProfilePicture = x.User.ProfilePicture,
            }).ToList();
            if (realtor.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<RealtorDto>>()
                {
                    Status = false,
                    Message = "No Information"
                };
            }

            return new BaseResponseModel<IEnumerable<RealtorDto>>()
            {
                Status = true,
                Data = realtor
            };
        }

        public async Task<BaseResponseModel<IEnumerable<BuyerDto>>> AllBuyers()
        {
            var buyers = await _buyerRepository.GetAllBuyers();
            if (!buyers.Any())
            {
                return new BaseResponseModel<IEnumerable<BuyerDto>>()
                {
                    Status = false,
                    Message = "No Registered Client"
                };
            }
            var buyerDtos = new List<BuyerDto>();
            
            foreach (var x in buyers)
            {
                buyerDtos.Add(new BuyerDto()
                {
                    Id = x.Id,
                    Mail = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    FName = $"{x.User.LastName} {x.User.FirstName}",
                    RegNo = x.RegId,
                    Address = x.Address,
                    ProfilePicture = x.User.ProfilePicture,
                });
            }

            return new BaseResponseModel<IEnumerable<BuyerDto>>()
            {
                Status = true,
                Data = buyerDtos
            };
        }

        public async Task<BaseResponseModel<IEnumerable<VisitationRequestDto>>> AllInspectionRequest()
        {
            var allInspection = await _visitationRepository.GetAll();
            if (allInspection == null)
            {
                return new BaseResponseModel<IEnumerable<VisitationRequestDto>>()
                {
                    Status = false,
                    Message = "No Property inspection request from any client yet"
                };
            }

            IList<VisitationRequestDto> visitationRequests = new List<VisitationRequestDto>();
            VisitationRequest[] inspection = allInspection.ToArray();
            foreach (var x in inspection)
            {
                visitationRequests.Add(new VisitationRequestDto()
                {
                    BuyerName = x.BuyerName,
                    BuyerPhoneNo = x.BuyerTelephone,
                    PropertyAddress = x.Address,
                    RequestDate = x.RequestDate,
                    PropertyRegNo = x.PropertyRegNo,
                    BuyerEmail = x.BuyerEmail
                });
            }

            return new BaseResponseModel<IEnumerable<VisitationRequestDto>>()
            {
                Status = true,
                Data = visitationRequests
            };
        }

        public async Task<BaseResponseModel<IEnumerable<PaymentDto>>> AllPayment()
        {
            var allPayment =await _paymentRepository.GetAllPayment();
            if (!allPayment.Any())
            {
                return new BaseResponseModel<IEnumerable<PaymentDto>>()
                {
                    Status = false,
                    Message = "No Payment Yet"
                };
            }
            var paymentDtos = new List<PaymentDto>();
            foreach (var payment in allPayment)
            {
                paymentDtos.Add(new PaymentDto()
                {
                    Id = payment.Id,
                    AgentId = payment.Property.Realtor.AgentId,
                    BuyerEmail = payment.BuyerEmail,
                    BuyerName = payment.BuyerName,
                    BuyerTelephone = payment.BuyerTelephone,
                    PaymentDate = payment.PaymentDate.ToString("g"),
                    PropertyPrice = payment.Amount,
                    TotalPrice = payment.TotalPrice,
                    PropertyType = payment.PropertyType,
                    TransactionId = payment.TransactionId,
                    PropertyRegNum = payment.Property.PropertyRegNo,
                    AgentEmail = payment.Property.Realtor.User.Email
                });
            }

            return new BaseResponseModel<IEnumerable<PaymentDto>>()
            {
                Status = true,
                Data = paymentDtos
            };
        }
        
        public async Task<BaseResponse> DeleteProperty(int propertyId)
        {
            var property = await  _propertyRepository.Get(propertyId);
            if (property == null)
            {
                return new BaseResponse()
                {
                    Status = false,
                    Message = "Failed to delete"
                };
            }

            var deleteProperty = await _propertyRepository.Delete(property);
            if (deleteProperty)
            {
                return new BaseResponse()
                {
                    Status = true,
                    Message = "Delete Successfully"
                };
            }
            else
            {
                return new BaseResponse()
                {
                    Status = false,
                    Message = "Failed to delete"
                };
            }
        }

    }
}