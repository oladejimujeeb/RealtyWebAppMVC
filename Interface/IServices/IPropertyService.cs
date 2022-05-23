using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;

namespace RealtyWebApp.Interface.IServices
{
    public interface IPropertyService
    {
        Task<BaseResponseModel<PropertyDto>> GetProperty(int id);
        Task<BaseResponseModel<IEnumerable<PropertyDto>>> GetAllPropertyWithImage();
        BaseResponseModel<IEnumerable<PropertyDto>> GetPropertyByRealtor(int realtyId);
        Task<BaseResponseModel<IEnumerable<PropertyDto>>> GetPropertyByBuyer(int buyerId);
    }
}