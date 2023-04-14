using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Repository
{
    public interface IRequestRepository
    {
        void Update(ReservationRequest request);
        ReservationRequest GetById(string requestId);
    }
}
