using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IPropertyRepository:IBaseRepository<Property>
    {
        Task<Property> GetWhere(string propertyId);
    }
}