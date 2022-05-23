using System.Threading.Tasks;
using RealtyWebApp.DTOs;

namespace RealtyWebApp.Interface.IServices
{
    public interface IPropertyImageService
    {
        Task<BaseResponseModel<PropertyImageDto>> GetPropertyImage(string propertyRegNo);
    }
}