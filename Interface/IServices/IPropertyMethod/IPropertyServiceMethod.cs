using System.Threading.Tasks;
using RealtyWebApp.DTOs;

namespace RealtyWebApp.Interface.IServices.IPropertyMethod
{
    public interface IPropertyServiceMethod
    {
        Task<BaseResponseModel<PropertyDto>> GetPropertyById(int id);
        Task<BaseResponseModel<PropertyDocumentDto>> DownloadPropertyDocument(int documentId);
    }
}