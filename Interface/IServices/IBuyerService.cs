using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IBuyerService
    {
        Task<BaseResponseModel<BuyerDto>> RegisterBuyer(BuyerRequestModel model);
    }
}