using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RealtyWebApp.DTOs;
using RealtyWebApp.DTOs.PayStack;
using RealtyWebApp.Entities;
using RealtyWebApp.Entities.File;
using RealtyWebApp.Entities.Identity;
using RealtyWebApp.Entities.Identity.Enum;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Interface.IServices.IPropertyMethod;
using RealtyWebApp.MailFolder.EmailService;
using RealtyWebApp.MailFolder.MailEntities;
using RealtyWebApp.Migrations;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Implementation.Services
{
    public class RealtorService:IRealtorService
    {
        private readonly IConfiguration _configuration;
        private readonly IRealtorRepository _realtorRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPropertyImage _propertyImage;
        private readonly IPropertyDocument _propertyDocument;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMailService _mailService;
        private readonly IPropertyServiceMethod _propertyServiceMethod;

        public RealtorService(IConfiguration configuration,IRealtorRepository realtorRepository, IUserRepository userRepository, IRoleRepository roleRepository, 
            IWebHostEnvironment webHostEnvironment, IPropertyImage propertyImage, IPropertyDocument propertyDocument, 
            IPropertyRepository propertyRepository, IWalletRepository walletRepository, IPropertyServiceMethod propertyServiceMethod,
            IPaymentRepository paymentRepository, IMailService mailService)
        {
            _configuration = configuration;
            _realtorRepository = realtorRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _webHostEnvironment = webHostEnvironment;
            _propertyImage = propertyImage;
            _propertyDocument = propertyDocument;
            _propertyRepository = propertyRepository;
            _walletRepository = walletRepository;
            _paymentRepository = paymentRepository;
            _mailService = mailService;
            _propertyServiceMethod = propertyServiceMethod;
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
            
            var basePath = Path.Combine(_webHostEnvironment.WebRootPath, "\\ProfilePictures\\");
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
                var wallet = new Wallet()
                { 
                    Realtor = realtor,
                    RealtorId = realtor.Id
                };
                await _walletRepository.Add(wallet);
                if (addRealtor == realtor)
                {
                    //sending mail upon registration
                    /*WelcomeMessage sendMail = new WelcomeMessage()
                    {
                        Email = user.Email,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Id = addRealtor.AgentId
                    };
                    //send mail
                    await _mailService.WelcomeMail(sendMail);*/
                    return new BaseResponseModel<RealtorDto>
                    {
                        Status = true,
                        Message = "Account Created Successfully",
                    };
                }
            }
            return new BaseResponseModel<RealtorDto>()
            {
                Message = "Registration failed, upload only image for profile picture ",
                Status = false
            };
        }

        public async Task<BaseResponseModel<RealtorDto>> UpdateRealtorInfo(RealtorUpdateRequest model, int id)
        {
            var userInfo = await _userRepository.Get(x => x.Id == id);
            var realtor = await _realtorRepository.Get(x => x.UserId == id);
            if (realtor == null || userInfo == null)
            {
                return new BaseResponseModel<RealtorDto>(){Message = "Failed to update", Status = false};
            }
            
            userInfo.FirstName = model.FirstName;
            userInfo.LastName = model.LastName;
            userInfo.PhoneNumber = model.PhoneNumber;
            realtor.Address = model.Address;
            realtor.BusinessName = model.BusinessName;
            await _userRepository.Update(userInfo);
            await _realtorRepository.Update(realtor);
            return new BaseResponseModel<RealtorDto>()
            {
                Status = true,
                Message = "Update Successfully",
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
                bool basePathExists = Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var extension = Path.GetExtension(image.FileName);
                var fileName =  $"IMG{Guid.NewGuid().ToString().Substring(0,4)}{extension}";
                var filePath = Path.Combine(basePath, fileName);
                
                if (!File.Exists(filePath) && extension==".jpg"|| extension ==".png"|| extension==".jpeg")
                {
                    await using (var stream = new FileStream(filePath, FileMode.Create))
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
                else
                {
                    return new BaseResponseModel<PropertyDto>()
                    {
                        Status = false,
                        Message = "Failed, Upload only image for property images"
                    };
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
                await using (var dataStream = new MemoryStream())
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
                /*Data =new PropertyDto()
                {
                    ImagePath = registerProperty.PropertyImages.Select(x=>x.DocumentName).ToList()
                }*/
                
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByRealtorId(int realtorId)
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.RealtorId == realtorId && x.BuyerIdentity==0 && !x.VerificationStatus).
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
                    RegisteredDate = x.RegisteredDate,
                    LGA = x.LGA,
                    State = x.State,
                    ImagePath = _propertyImage.QueryWhere(y=>y.PropertyRegNo==x.PropertyRegNo).Select(y=>y.DocumentName).ToList()
                }).ToList();
            if (getProperty.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<PropertyDto>>()
                {
                    Status = false,
                    Message = "No Unapproved property to list"
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
                    LGA = x.LGA,
                    State = x.State,
                }).ToList();
            if (getProperty.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<PropertyDto>>()
                {
                    Status = false,
                    Message = "We have not sold any of your properties, You many contact the Admin for more infomation"
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
            var getProperty = _propertyRepository.QueryWhere(x => x.RealtorId == id && x.VerificationStatus && !x.IsSold).
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
                    RegisteredDate = x.RegisteredDate,
                    ImagePath = _propertyImage.QueryWhere(y=>y.PropertyRegNo==x.PropertyRegNo).Select(y=>y.DocumentName).ToList()
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

        public async Task<BaseResponseModel<PropertyDto>> GetProperty(int id)
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

        public async Task<BaseResponseModel<RealtorDto>> GetUser(int id)
        {
            var userInfo = await _userRepository.Get(x => x.Id == id);
            var realtor = await _realtorRepository.Get(x => x.UserId == userInfo.Id);
            if (userInfo == null)
            {
                return new BaseResponseModel<RealtorDto>()
                {
                    Status = false,
                    Message = "failed"
                };
            }

            return new BaseResponseModel<RealtorDto>()
            {
                Status = true,
                Data = new RealtorDto()
                {
                  Mail  = userInfo.Email,
                  PhoneNumber = userInfo.PhoneNumber,
                  Address = realtor.Address,
                  BusinessName = realtor.BusinessName,
                  FName = userInfo.FirstName,
                  LName = userInfo.LastName,
                }
            };
        }

        public async Task<BaseResponseModel<BaseResponseModel<PropertyDto>>> EditProperty(int propertyId, UpdatePropertyModel updateProperty)
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
            getProperty.Toilet = updateProperty.Toilet;
            getProperty.BuildingType = updateProperty.BuildingType;
            getProperty.PlotArea = updateProperty.PlotArea;
            getProperty.PropertyType = updateProperty.PropertyType;
            getProperty.LGA = updateProperty.LGA;
            getProperty.State = updateProperty.State;
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
         public async Task<BaseResponse> AddAccount(int realtorId, TransferRequest request)
         {
             var upDatedWallet = await _walletRepository.Get(x => x.RealtorId == realtorId);
             if (upDatedWallet == null) return new BaseResponse() { Status = false, Message = "Invalid login Credential "};
             //if (upDatedWallet.ReceipientCode!=null) return new BaseResponse() { Status = false, Message = "Account Details Already Added "};
             using var httpClient = new HttpClient();
             httpClient.DefaultRequestHeaders.Accept.Clear();
             httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
             httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                 ("Bearer", _configuration["PayStack:SecretKey"]);
             httpClient.BaseAddress = new Uri("https://api.paystack.co/");
             var accountDetails = new StringContent(JsonConvert.SerializeObject(new
             {
                 type = "nuban",
                 name = request.AccountName,
                 account_number =request.AccountNo,
                 bank_code = request.BankCode,
                 currency = "NGN"
             }), Encoding.UTF8, "application/json");
             var addAccount = await httpClient.PostAsync("transferrecipient",accountDetails);
             var addResponse = await addAccount.Content.ReadAsStringAsync();
             if (addAccount.IsSuccessStatusCode)
             {
                 var responseDetails = JsonConvert.DeserializeObject<AddAccountResponse>(addResponse);
                 if (responseDetails.data.active == "true")
                 {

                     upDatedWallet.ReceipientCode = responseDetails.data.recipient_code;
                     upDatedWallet.AccountName = responseDetails.data.details.account_name;
                     upDatedWallet.AccountNo = responseDetails.data.details.account_number;
                     upDatedWallet.BankName = responseDetails.data.details.bank_name;
                     var add = await _walletRepository.Update(upDatedWallet);
                     if(add ==null) return new BaseResponse() { Status = false, Message = "Failed"};
                     return new BaseResponse()
                     {
                         Status = true,
                         Message = "Account Added Successfully"
                     };
                 }
                 return new BaseResponse(){
                     Status = false,
                     Message = "Wrong Account number or Account Name"
                 };
             }
            
             return new BaseResponse()
             {
                 Status = false,
                 Message = "Wrong Account number or Account Name"
             };
            
         }

         public async Task<BaseResponseModel<WalletDto>> GetWallet(int realtorId)
         {
             var getWallet = await _walletRepository.Get(x => x.RealtorId == realtorId);
             if (getWallet == null)
             {
                 return new BaseResponseModel<WalletDto>()
                 {
                     Status = false,
                     Message = "Invalid login Credential"
                 };
             }

             return new BaseResponseModel<WalletDto>()
             {
                 Status = true,
                 Data = new WalletDto()
                 {
                     AccountBalance = getWallet.AccountBalance,
                     AccountName = getWallet.AccountName,
                     BankName = getWallet.BankName,
                     AccountNo = getWallet.AccountNo
                 }
             };
         }
    }
}