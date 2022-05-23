using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Interface.IRepositories;
using RealtyWebApp.Interface.IServices;

namespace RealtyWebApp.Implementation.Services
{
    public class PropertyImageService:IPropertyImageService
    {
        public Task<BaseResponseModel<PropertyImageDto>> GetPropertyImage(string propertyRegNo)
        {
            throw new System.NotImplementedException();
        }
    }
}