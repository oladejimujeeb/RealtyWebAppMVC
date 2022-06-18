using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Implementation.Repositories;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IServices
{
    public interface IUserService
    {
        Task<BaseResponseModel<UserDto>> GetUser(LoginModel model);
        Task<BaseResponseModel<UserDto>> GetUserById(int id);
    }
}