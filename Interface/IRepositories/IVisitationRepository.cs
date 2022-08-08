using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealtyWebApp.DTOs;
using RealtyWebApp.Entities;

namespace RealtyWebApp.Interface.IRepositories
{
    public interface IVisitationRepository:IBaseRepository<VisitationRequest>
    {
        IEnumerable<VisitationRequest> Visitation();
        Task<IEnumerable<VisitationRequest>> BuyerInspectedProperty(int buyerId);
        int CheckIfDateIsAvailable(DateTime date);
    }
}