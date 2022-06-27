using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IRealtorService
    {
        Task<BaseResponseModel<RealtorDto>> RegisterRealtor(RealtorRequestModel model);
        Task<BaseResponseModel<RealtorDto>> UpdateRealtorInfo(RealtorUpdateRequest model, int id);
        Task<BaseResponseModel<PropertyDto>> AddProperty(PropertyRequestModel model, int id);
        BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByRealtorId(int realtorId);
        BaseResponseModel<IEnumerable<PropertyDto>> GetSoldPropertyByRealtor(int realtorId);
        BaseResponseModel<IEnumerable<PropertyDto>> GetRealtorApprovedProperty(int id);
        Task<BaseResponseModel<PropertyDto>> GetProperty(int id);
        Task<BaseResponseModel<RealtorDto>> GetUser(int id);
        Task<BaseResponseModel<BaseResponseModel<PropertyDto>>> EditProperty(int propertyId, UpdatePropertyModel updateProperty);
    }
}