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
using RealtyWebApp.MailFolder.EmailService;
using RealtyWebApp.MailFolder.MailEntities;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Implementation.Services
{
    public class BuyerService:IBuyerService
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IVisitationRepository _visitationRepository;
        private readonly IPropertyImage _propertyImage;
        private readonly IPropertyDocument _propertyDocument;
        private readonly IMailService _mailService;
        public BuyerService(IBuyerRepository buyerRepository,IMailService mailService, IRoleRepository roleRepository, IUserRepository userRepository, IPropertyRepository propertyRepository, IVisitationRepository visitationRepository, IPropertyImage propertyImage, IPropertyDocument propertyDocument)
        {
            _buyerRepository = buyerRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _visitationRepository = visitationRepository;
            _propertyImage = propertyImage;
            _propertyDocument = propertyDocument;
            _mailService = mailService;
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
                //User = user,
                UserId = user.Id,
                RoleId = getRole.Id,
            };
            user.UserRoles.Add(userRole);
            var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\ProfilePictures\\");
            bool basePathExists = Directory.Exists(basePath);
            if (!basePathExists)
            {
                Directory.CreateDirectory(basePath);
            }
            var fileName = Path.GetFileNameWithoutExtension(model.ProfilePicture.FileName);
            var filePath = Path.Combine(basePath, model.ProfilePicture.FileName);
            var extension = Path.GetExtension(model.ProfilePicture.FileName);
            if (!File.Exists(filePath)&& extension==".jpg"|| extension ==".png"|| extension==".jpeg")
            {
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = fileName+extension;
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
            WelcomeMessage sendMail = new WelcomeMessage()
            {
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Id = buyer.RegId
            };
            //send mail
            //await _mailService.WelcomeMail(sendMail);
            return new BaseResponseModel<BuyerDto>()
            {
                Status = true,
                Message = "Registration  Successful",
                Data = new BuyerDto()
                {
                    RegNo = addBuyer.RegId
                }
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByBuyer(int buyerId)
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.BuyerIdentity == buyerId && x.IsAvailable && x.IsSold).
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
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    LGA = x.LGA,
                    State = x.State,
                    ImagePath = _propertyImage.QueryWhere(y=>y.PropertyRegNo==x.PropertyRegNo).Select(y=>y.DocumentName).ToList() 
                }).ToList();
            if (getProperty.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<PropertyDto>>()
                {
                    Status = false,
                    Message = "You have not purchased any property yet"
                };
            }
            
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }

        public async Task<BaseResponseModel<VisitationRequestDto>> MakeVisitationRequest(int buyerId, int propertyId)
        {
            var getBuyer =await _userRepository.Get(x => x.Buyer.Id == buyerId);
            var getProperty = await _propertyRepository.Get(x => x.Id == propertyId);
            var request = new VisitationRequest()
            {
                BuyerEmail = getBuyer.Email,
                BuyerId = buyerId,
                BuyerName = $"{getBuyer.FirstName } {getBuyer.LastName}",
                BuyerTelephone = getBuyer.PhoneNumber,
                PropertyId = getProperty.Id,
                PropertyType = getProperty.PropertyType,
                PropertyRegNo = getProperty.PropertyRegNo,
                Address = $"{getProperty.Address} {getProperty.LGA} {getProperty.State}",
            };
            var date = DateTime.Now.AddDays(3);
            
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
               var newDate = date.AddDays(1);
               request.RequestDate = newDate;
            }
            else
            {
                request.RequestDate = date;
            }

            //getProperty.BuyerIdentity = buyerId;
            var addVisitation =await _visitationRepository.Add(request);
            if (addVisitation != request)
            {
                return new BaseResponseModel<VisitationRequestDto>()
                {
                    Status = false,
                    Message = "Fail to Schedule date called Customer Service",
                };
            }
            var scheduledDate = date;
            string visitDate = scheduledDate.ToString("dddd,dd MMMM yyyy");
            
            return new BaseResponseModel<VisitationRequestDto>()
            {
                Data =new VisitationRequestDto()
                {
                    Id = request.BuyerId,
                    PropertyRegNo = request.PropertyRegNo,
                    PropertyAddress = request.Address,
                    Message = $"Kind Visit Our office on {visitDate} for Property Inspection," +
                              $" If Date is not convenient call Our Customer Service on 08136794915 to reschedule" +
                              $" or Send a mail to us on oladejimujib@gmail.com ",
                    BuyerName = request.BuyerName,
                    BuyerPhoneNo = request.BuyerTelephone,
                    PropertyPrice = getProperty.Price,
                    Mail = request.BuyerEmail
                },
                Status = true,
                Message = $"Kind Visit Our office on {visitDate} for Property Inspection," +
                          $" If Date is not convenient call Our Customer Service on 08136794915 to reschedule ",
            };
        }

        public async Task<BaseResponseModel<PropertyDocumentDto>> DownloadPropertyDocument(int documentId)
        {
            var file = await _propertyDocument.Get(x => x.Property.Id == documentId);//Possible Error
            if (file == null)
            {
                return new BaseResponseModel<PropertyDocumentDto>()
                {
                    Status = false,
                    Message = "File failed to download"
                };
            }

            return new BaseResponseModel<PropertyDocumentDto>
            {
                Data = new PropertyDocumentDto()
                {
                    Extension = file.Extension,
                    DocumentPath = file.DocumentName,
                    FileType = file.FileType,
                    Data = file.Data,
                    PropertyRegNo = file.PropertyRegNo
                },
                Status = true
            };
        }

        public BaseResponseModel<IEnumerable<VisitationRequestDto>> ListOfRequestedProperties(int buyerId)
        {
            var visitationRequest = _visitationRepository.QueryWhere(x => x.BuyerId == buyerId).
                Select(x=>new VisitationRequestDto
                {
                    Id = x.Id,
                    BuyerId = x.BuyerId,
                    BuyerName = x.BuyerName,
                    PropertyAddress = x.Address,
                    PropertyId = x.PropertyId,
                    RequestDate = x.RequestDate,
                    PropertyRegNo = x.PropertyRegNo,
                    PropertyType = x.PropertyType,
                    BuyerPhoneNo = x.BuyerTelephone
                }).ToList();
            
            if (visitationRequest.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<VisitationRequestDto>>
                {
                    Status = false,
                    Message = "You have not make any request"
                };
            }
            return new BaseResponseModel<IEnumerable<VisitationRequestDto>>
            {
                Status = true,
                Data = visitationRequest
            };
            
        }
    }
}