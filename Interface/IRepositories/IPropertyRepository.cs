using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IPropertyRepository:IBaseRepository<Property>
    {
        Task<Property> GetWhere(string propertyId);
        Task<IList<Property>> SearchWhere(SearchRequest search);
        IEnumerable<Property> PurchasedProperty(int userId);
        List<Property> AllUnverifiedProperty();
        List<Property> AllVerifiedProperty();
    }
}