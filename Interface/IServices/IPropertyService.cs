using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IPropertyService
    {
        Task<BaseResponseModel<PropertyDto>> GetProperty(int id);
        BaseResponseModel<IEnumerable<PropertyDto>> AllAvailablePropertyWithImage();
        /*BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByRealtor(int realtyId);
        BaseResponseModel<IEnumerable<PropertyDto>> GetSoldPropertyByRealtor(int realtyId);
        BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByBuyer(int buyerId);
        BaseResponseModel<IEnumerable<PropertyDto>> GetRealtorApprovedProperty(int id);
        BaseResponseModel<IEnumerable<PropertyDto>> AllUnverifiedProperty();*/
        Task<BaseResponseModel<IEnumerable<PropertyDto>>> SearchProperty(SearchRequest model);
    }
}