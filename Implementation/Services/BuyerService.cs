using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RealtyWebApp.DTOs;
using RealtyWebApp.DTOs.PayStack;
using RealtyWebApp.Entities;
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
    public class BuyerService:IBuyerService
    {
        private readonly IConfiguration _configuration;
        private readonly IBuyerRepository _buyerRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IVisitationRepository _visitationRepository;
        private readonly IPropertyImage _propertyImage;
        private readonly IPropertyServiceMethod _propertyServiceMethod;
        private readonly IMailService _mailService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IWalletRepository _walletRepository;

        public BuyerService(IConfiguration configuration, IBuyerRepository buyerRepository, IRoleRepository roleRepository, IUserRepository userRepository, 
            IPropertyRepository propertyRepository, IVisitationRepository visitationRepository, IPropertyServiceMethod propertyServiceMethod,
            IPropertyImage propertyImage, IMailService mailService, IPaymentRepository paymentRepository,
            IWalletRepository walletRepository)
        {
            _configuration = configuration;
            _buyerRepository = buyerRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _visitationRepository = visitationRepository;
            _propertyImage = propertyImage;
            _propertyServiceMethod = propertyServiceMethod;
            _mailService = mailService;
            _paymentRepository = paymentRepository;
            _walletRepository = walletRepository;
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
            var getProperty = _propertyRepository.PurchasedProperty(buyerId).
                Select(x=>new PropertyDto()
                {
                    Id = x.Id,
                    Address = $"{x.Address}, {x.LGA}, {x.State}",
                    Features = x.Features,
                    PropertyType = x.PropertyType,
                    BuyerId = x.BuyerIdentity,
                    LandArea = x.PlotArea,
                    PropertyPrice = x.Payment.TotalPrice,
                    PropertyRegNumber = x.PropertyRegNo,
                    PropertyRegNo = x.PropertyRegNo,
                    SoldDate = x.Payment.PaymentDate,
                    PaymentReference = x.Payment.TransactionId
                    //ImagePath = _propertyImage.QueryWhere(y=>y.PropertyRegNo==x.PropertyRegNo).Select(y=>y.DocumentName).ToList() 
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
            if (getProperty == null || getBuyer == null)
            {
                return new BaseResponseModel<VisitationRequestDto>()
                {
                    Status = false,
                    Message = "Failed",
                };
            }
            var alreadyMakeRequest = await _visitationRepository.Get(x => x.BuyerEmail == getBuyer.Email &&
                                                                   x.PropertyRegNo == getProperty.PropertyRegNo);
            if (alreadyMakeRequest != null)
            {
                return new BaseResponseModel<VisitationRequestDto>()
                {
                    Status = false,
                    Message = $"A date has already been scheduled for you for inspection of this particular property with Id: {alreadyMakeRequest.PropertyRegNo}." +
                              $"Call or mail our customer service if you need to reschedule the date"
                };
            }
            
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
            
            var date = GenerateVisitationDateTime();
            var check = _visitationRepository.CheckIfDateIsAvailable(date);
            if (check > 5)
            {
                var dateTime = date.AddDays(3);
                date = dateTime;
            }

            request.RequestDate = date;
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
            var visitDate = scheduledDate.ToString("dddd,dd MMMM yyyy");
            
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
                    Mail = request.BuyerEmail,
                    PropertyType = request.PropertyType
                },
                Status = true,
                Message = $"Kind Visit Our office on {visitDate} for Property Inspection," +
                          $" If Date is not convenient call Our Customer Service on 08136794915 to reschedule ",
            };
        }

        private DateTime GenerateVisitationDateTime()
        {
            var date = DateTime.Now.AddDays(3);
            
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.AddDays(1);
                
            }
            else if (date.DayOfWeek==DayOfWeek.Saturday)
            {
                return date.AddDays(2);
            }
            else
            {
                return date;
            }

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

        public async Task< BaseResponseModel<IEnumerable<VisitationRequestDto>>>ListOfBuyerVisitedProperty(int buyerId)
        {
            IList<VisitationRequestDto> visitationRequests = new List<VisitationRequestDto>();
            var propertyInspected = await _visitationRepository.BuyerInspectedProperty(buyerId);
            foreach (var x in propertyInspected)
            {
                visitationRequests.Add(new VisitationRequestDto()
                {
                    BuyerName = x.BuyerName,
                    BuyerPhoneNo = x.BuyerTelephone,
                    PropertyAddress = x.Address,
                    RequestDate = x.RequestDate,
                    PropertyRegNo = x.PropertyRegNo,
                    BuyerEmail = x.BuyerEmail,
                    PropertyPrice = x.Property.Price,
                    Mail = x.BuyerEmail,
                    PropertyId = x.PropertyId,
                    PropertyType = x.PropertyType,
                    BuyerId = x.BuyerId
                });
            }
            
            if (visitationRequests.Count == 0)
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
                Data = visitationRequests
            };
            
        }

        public async Task<BaseResponseModel<PropertyDto>> GetProperty(string propertyId)
        {
            var property = await _propertyRepository.GetWhere(propertyId);
            if (property == null)
            {
                return new BaseResponseModel<PropertyDto>()
                {
                    Status = false,
                    Message = "The Property You Are Looking Has been Sold to Another Client, We Are sorry About That."
                };
            }

            return new BaseResponseModel<PropertyDto>()
            {
                Status = true,
                Data = new PropertyDto()
                {
                    Id = property.Id,
                    Address = property.Address,
                    Bedroom = property.Bedroom,
                    Features = property.Features,
                    Latitude = property.Latitude,
                    Longitude = property.Longitude,
                    Toilet = property.Toilet,
                    BuildingType = property.BuildingType,
                    BuyerId = property.BuyerIdentity,
                    IsSold = property.IsSold,
                    LandArea = property.PlotArea,
                    PropertyPrice = property.Price,
                    RealtorId = property.RealtorId,
                    PropertyType = property.PropertyType,
                    PropertyRegNumber = property.PropertyRegNo,
                    LGA = property.LGA,
                    State = property.State,
                    ImagePath = _propertyImage.QueryWhere(y=>y.PropertyRegNo==property.PropertyRegNo).Select(y=>y.DocumentName).ToList()
                }
            };
        }

        public async Task<BaseResponseModel<BuyerDto>> GetBuyer(int id)
        {
            var buyerInfo = await _userRepository.GetUserBuyer(id);
            if (buyerInfo == null)
            {
                return new BaseResponseModel<BuyerDto>()
                {
                    Status = false,
                    Message = "Failed"
                };
            }

            return new BaseResponseModel<BuyerDto>()
            {
                Status = true,
                Data = new BuyerDto()
                {
                    Id = buyerInfo.Id,
                    Address = buyerInfo.Buyer.Address,
                    FName = buyerInfo.FirstName,
                    LName = buyerInfo.LastName,
                    PhoneNumber = buyerInfo.PhoneNumber,
                }
            };
        }

        public async Task<BaseResponseModel<BuyerDto>> UpdateBuyer(int id,UpdateBuyerModel model)
        {
            var userInfo = await _userRepository.Get(x => x.Id == id);
            var buyer = await _buyerRepository.Get(x => x.UserId == id);
            if (userInfo == null || buyer == null)
            {
                return new BaseResponseModel<BuyerDto>()
                {
                    Status = false,
                    Message = "Failed"
                };
            }

            userInfo.FirstName = model.FirstName;
            userInfo.LastName = model.LastName;
            userInfo.PhoneNumber = model.PhoneNumber;
            buyer.Address = model.Address;
            var updateUser = await _userRepository.Update(userInfo);
            var updateBuyer = await _buyerRepository.Update(buyer);
            if (updateBuyer == null || updateUser == null)
            {
                return new BaseResponseModel<BuyerDto>()
                {
                    Status = false,
                    Message = "Failed to Update"
                };
            }

            return new BaseResponseModel<BuyerDto>()
            {
                Status = true,
                Message = "Update Successfully"
            };
        }

        public async Task<BaseResponseModel<PaymentBreakDown>> PaymentBreakDown(int propertyId, int buyerId)
        {
            var property = await _propertyRepository.Get(propertyId);
            var getBuyer =await _userRepository.Get(x => x.Id == buyerId);
            if (property == null || getBuyer==null)
            {
                return new BaseResponseModel<PaymentBreakDown>()
                {
                    Status = false,
                    Message = "Failed!! The Property You Are Looking Has been Sold to Another Client, We Are sorry About That."
                };
            }

            var paymentBreakDown = new PaymentBreakDown()
            {
                PropertyId = property.Id,
                PropertyPrice = property.Price,
                PropertyType = property.PropertyType,
                BuyerName = $"{getBuyer.LastName} {getBuyer.FirstName}",
                BuyerEmail = getBuyer.Email,
                BuyerTelephone = getBuyer.PhoneNumber,
                PropertyRegNumber = property.PropertyRegNo
            };
            if (property.Price <= 750000)
            {
                paymentBreakDown.AgencyFees = property.Price * 0.10;
                paymentBreakDown.AgreementFees = 30000;
                paymentBreakDown.TotalPrice = paymentBreakDown.AgencyFees + paymentBreakDown.AgreementFees + property.Price; 
            }
            else if (property.Price >750000 && property.Price <=5000000)
            {
                paymentBreakDown.AgencyFees = property.Price * 0.05;
                paymentBreakDown.AgreementFees = property.Price * 0.04;
                paymentBreakDown.TotalPrice = paymentBreakDown.AgencyFees + paymentBreakDown.AgreementFees + property.Price;
            }
            else
            {
                paymentBreakDown.AgencyFees = property.Price * 0.04;
                paymentBreakDown.AgreementFees = property.Price * 0.05;
                paymentBreakDown.TotalPrice = paymentBreakDown.AgencyFees + paymentBreakDown.AgreementFees + property.Price;
            }

            return new BaseResponseModel<PaymentBreakDown>()
            {
                Status = true,
                Data = new PaymentBreakDown()
                {
                    PropertyId = paymentBreakDown.PropertyId,
                    AgencyFees = paymentBreakDown.AgencyFees,
                    AgreementFees = paymentBreakDown.AgreementFees,
                    BuyerEmail = paymentBreakDown.BuyerEmail,
                    BuyerName = paymentBreakDown.BuyerName,
                    BuyerTelephone = paymentBreakDown.BuyerTelephone,
                    PropertyPrice = paymentBreakDown.PropertyPrice,
                    PropertyType = paymentBreakDown.PropertyType,
                    TotalPrice = paymentBreakDown.TotalPrice,
                    PropertyRegNumber = paymentBreakDown.PropertyRegNumber
                }
            };
        }
        
        public async Task<BaseResponse> MakePayment(int buyerId, PaymentRequestModel paymentRequest)
        {
            var property = await _propertyRepository.GetWhere(paymentRequest.PropertyRegNumber);
            var payment = new Payment()
            {
                Amount = paymentRequest.Amount,
                BuyerEmail = paymentRequest.BuyerEmail,
                BuyerName = paymentRequest.BuyerName,
                BuyerTelephone = paymentRequest.BuyerTelephone,
                TotalPrice = paymentRequest.TotalPrice,
                PropertyType = paymentRequest.PropertyType,
                PropertyId = property.Id,
                TransactionId = $"REALTY-PRTY-{Guid.NewGuid().ToString().Substring(0,9)}" ,
            };
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri("https://api.paystack.co/transaction/initialize");
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer",_configuration["PayStack:SecretKey"]);
            
            var content = new StringContent(JsonConvert.SerializeObject( new
            {
                amount = paymentRequest.TotalPrice * 100,
                email = paymentRequest.BuyerEmail,
                reference =payment.TransactionId,
                currency = "NGN",
                callback_url = "https://localhost:5001/Buyer/VerifyPayment"
                
            }), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://api.paystack.co/transaction/initialize", content);
            var responseToString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode==System.Net.HttpStatusCode.OK)
            {
                payment.PaymentDate = DateTime.Now;
                var savePayment =  await _paymentRepository.Add(payment);
                property.BuyerIdentity = buyerId;
                /*property.IsSold = true;
                property.Status = Status.Sold.ToString();
                property.IsAvailable = false;*/
                var updatePropertySale = await _propertyRepository.Update(property);
                if (savePayment == null && updatePropertySale == null)
                {
                    return new BaseResponse()
                    {
                        Status = false,
                        Message = "Payment Failed"
                    };
                }

                var responseObject = JsonConvert.DeserializeObject<PayStackResponse>(responseToString);
                if (responseObject.status)
                {
                    return new BaseResponse()
                    {
                        Status = true,
                        Message = responseObject.data.authorization_url
                    };
                    
                }
                return new BaseResponse()
                {
                    Status = false,
                    Message = responseObject.message
                };
            }
            else
            {
                return new BaseResponse()
                {
                    Status = false,
                    Message = response.ReasonPhrase
                };
            }
        }
        
        public async Task<BaseResponse> VerifyPayment(string transactionReference)
        {
            var verify = await _paymentRepository.GetPayment(transactionReference);
            if (verify == null)
            {
                return new BaseResponse()
                {
                    Status = false,
                    Message = "Payment Not Completed"
                };
            }

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri("https://api.paystack.co/transaction/verify/");
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer",_configuration["PayStack:SecretKey"]);
            var response = await httpClient.GetAsync(transactionReference);
            var responseToString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var responseObject = JsonConvert.DeserializeObject<PayStackResponse>(responseToString);
                if (responseObject.data.status == "success")
                {
                    verify.Status = PaymentStatus.Success;
                    verify.Property.IsSold = true;
                    verify.Property.Status = Status.Sold.ToString();
                    verify.Property.IsAvailable = false;
                    verify.Property.Realtor.Wallet.AccountBalance += verify.Amount;
                   
                    var updatePayment = await _paymentRepository.Update(verify);
                    await _walletRepository.Update(verify.Property.Realtor.Wallet);
                    await _propertyRepository.Update(verify.Property);
                    
                    if (updatePayment == null)
                    {
                        return new BaseResponse()
                        {
                            Status = false,
                            Message = "Something Went Wrong"
                        };
                    }

                    return new BaseResponse()
                    {
                        Status = true,
                        Message = "Payment Received Successfully"
                    };
                }
            }

            return new BaseResponse()
            {
                Status = false,
                Message = "Payment Was Not Successful"
            };

        }
    }
}