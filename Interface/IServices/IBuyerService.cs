using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IBuyerService
    {
        Task<BaseResponseModel<BuyerDto>> RegisterBuyer(BuyerRequestModel model);
        BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByBuyer(int buyerId);
        Task<BaseResponseModel<VisitationRequestDto>> MakeVisitationRequest(int buyerId, int propertyId);
        Task<BaseResponseModel<PropertyDocumentDto>> DownloadPropertyDocument(int documentId);
        BaseResponseModel<IEnumerable<VisitationRequestDto>> ListOfRequestedProperties(int buyerId);
    }
}