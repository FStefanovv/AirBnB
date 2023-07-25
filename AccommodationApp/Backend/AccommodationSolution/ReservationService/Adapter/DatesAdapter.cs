using ReservationService.DTO;
using System;
using System.Collections.Generic;

namespace ReservationService.Adapter
{
    public class DatesAdapter
    {

        public static StartDateDTO ObjectToStartDateDTO (List<DateTime> dateTime)
        {
            StartDateDTO dto = new StartDateDTO();

            for (int i = 0; i < dateTime.Count; i++) 
            dto.StartDate[i] = dateTime[i].ToString("dd-MM-yyyy");

            return dto;
        }

        public static EndDateDTO ObjectToEndDateDTO(List<DateTime> dateTime)
        {
            EndDateDTO dto = new EndDateDTO();

            for (int i = 0; i < dateTime.Count; i++)
                dto.EndDate[i] = dateTime[i].ToString("dd-MM-yyyy");

            return dto;
        }
    }
}
