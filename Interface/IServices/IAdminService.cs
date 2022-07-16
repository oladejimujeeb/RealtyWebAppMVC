using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Implementation.Repositories;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IAdminService
    {
        Task<BaseResponseModel<AdminDto>> RegisterAdmin(AdminRequestModel model);
        Task<BaseResponseModel<PropertyDto>> GetPropertyById(int id);
        BaseResponseModel<IEnumerable<PropertyDto>> AllUnverifiedProperty();
        BaseResponseModel<IEnumerable<VisitationRequestDto>> VisitationRequest();
        Task<BaseResponseModel<BaseResponseModel<PropertyDto>>> UpdateRealtorPropertyForSale(int propertyId, UpdatePropertyModel updateProperty);
        BaseResponseModel<IEnumerable<PropertyDto>> SearchPropertyByRegNo(SearchRequest searchRequest);
        BaseResponseModel<IEnumerable<PropertyDto>> AllVerifiedProperty();
        BaseResponseModel<IEnumerable<PropertyDto>> AllSoldProperty();
        BaseResponseModel<IEnumerable<RealtorDto>> AllRealtors();
        Task<BaseResponseModel<IEnumerable<BuyerDto>>> AllBuyers();
        Task<BaseResponseModel<IEnumerable<VisitationRequestDto>>> AllInspectionRequest();
        Task<BaseResponseModel<PropertyDocumentDto>> DownloadPropertyDocument(int documentId);
        Task<BaseResponseModel<IEnumerable<PaymentDto>>> AllPayment();
    }
}