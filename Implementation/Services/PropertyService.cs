using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;

namespace RealtyWebApp.Implementation.Services
{
    public class PropertyService:IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImage _propertyImage;

        public PropertyService(IPropertyRepository propertyRepository, IPropertyImage propertyImage)
        {
            _propertyRepository = propertyRepository;
            _propertyImage = propertyImage;
        }
        public async Task<BaseResponseModel<PropertyDto>> GetProperty(int id)
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
                    //ImagePath = getProperty.PropertyImages.Select(z=>z.DocumentPath).ToList(),
                },
                Message = "load successfully"
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> AllAvailablePropertyWithImage()
        {
            var getProperty =  _propertyRepository.QueryWhere(x=>x.IsAvailable && x.VerificationStatus && x.BuyerIdentity==0).
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
                    ImagePath = _propertyImage.QueryWhere(y=>y.PropertyRegNo==x.PropertyRegNo).Select(y=>y.DocumentName).ToList()
                }).ToList();
            if (getProperty.Count == 0)
            {
                return new BaseResponseModel<IEnumerable<PropertyDto>>()
                {
                    Status = false,
                    Message = "No Available Property"
                };
            }

            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data = getProperty
            };
        }

        public  BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByRealtor(int realtorId)
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
                    //PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    ImagePath = x.PropertyImages.Select(z=>z.DocumentName).ToList(),//Possible Error
                }).ToList();
            
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
                    //PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    IsSold = x.IsSold,
                }).ToList();
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }

        public BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByBuyer(int buyerId)
        {
            var getProperty = _propertyRepository.QueryWhere(x => x.BuyerIdentity == buyerId && x.IsAvailable).
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
                    //PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    //ImagePath = x.PropertyImages.Select(z=>z.DocumentPath).ToList(),//Possible Error
                }).ToList();
            
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
                    //PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    //ImagePath = x.PropertyImages.Select(z=>z.DocumentPath).ToList(),//Possible Error
                }).ToList();
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
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
                    //PropertyRegNumber = x.PropertyRegNo,
                    Action = x.Action,
                    Status = x.Status,
                    VerificationStatus = x.VerificationStatus,
                    IsAvailable = x.IsAvailable,
                    PropertyRegNo = x.PropertyRegNo,
                    IsSold = x.IsSold,
                    ImagePath = x.PropertyImages.Select(z=>z.DocumentName).ToList(),//Possible Error
                }).ToList();
            return new BaseResponseModel<IEnumerable<PropertyDto>>()
            {
                Status = true,
                Data =getProperty
            };
        }
    }
}