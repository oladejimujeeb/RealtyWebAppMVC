using System.Linq;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices.IPropertyMethod;

namespace RealtyWebApp.Implementation.Services.PropertyMethod
{
    public class PropertyServiceMethod:IPropertyServiceMethod
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImage _propertyImage;
        private readonly IPropertyDocument _propertyDocument;
        public PropertyServiceMethod(IPropertyRepository propertyRepository, IPropertyImage propertyImage, IPropertyDocument propertyDocument)
        {
            _propertyRepository = propertyRepository;
            _propertyImage = propertyImage;
            _propertyDocument = propertyDocument;
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
                    ImagePath = _propertyImage.QueryWhere(y=>y.PropertyRegNo==getProperty.PropertyRegNo).Select(y=>y.DocumentName).ToList(),
                },
                Message = "load successfully"
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
                    DocumentPath = file.Description,
                    FileType = file.FileType,
                    Data = file.Data,
                    PropertyRegNo = file.PropertyRegNo
                },
                Status = true
            };
        }
    }
}