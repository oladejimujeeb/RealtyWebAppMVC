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
        Task<BaseResponseModel<IEnumerable<VisitationRequestDto>>> ListOfBuyerVisitedProperty(int buyerId);
        Task<BaseResponseModel<PropertyDto>> GetProperty(string propertyId);
        Task<BaseResponseModel<BuyerDto>> GetBuyer(int id);
        Task<BaseResponseModel<BuyerDto>> UpdateBuyer(int id, UpdateBuyerModel model);
        Task<BaseResponseModel<PaymentBreakDown>> PaymentBreakDown(int propertyId, int buyerId);
        Task<BaseResponse> MakePayment(int buyerId, PaymentRequestModel paymentRequest);
        Task<BaseResponse> VerifyPayment(string transactionReference);
    }
}