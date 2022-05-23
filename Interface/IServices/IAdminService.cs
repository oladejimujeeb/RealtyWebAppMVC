using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IAdminService
    {
        Task<BaseResponseModel<AdminDto>> RegisterAdmin(AdminRequestModel model);
    }
}