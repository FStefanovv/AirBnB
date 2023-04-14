using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService.Service
{
    public interface IRequestService
    {
        void CancelReservationRequest(string id, StringValues userId);
    }
}
