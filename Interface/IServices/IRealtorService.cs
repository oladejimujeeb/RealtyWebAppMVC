using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IRealtorService
    {
        Task<BaseResponseModel<RealtorDto>> RegisterRealtor(RealtorRequestModel model);
        Task<BaseResponseModel<PropertyDto>> AddProperty(PropertyRequestModel model, int id);
    }
}