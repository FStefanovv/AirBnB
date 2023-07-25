using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.DTO
{
    public class RatingWithUsernameDTO
    {
        public int Grade { get; set; }
        public DateTime RatingDate { get; set; }
        public string Username { get; set; }


        public RatingWithUsernameDTO() { }

        public RatingWithUsernameDTO(string username, int grade)
        {
            Grade = grade;
            RatingDate = DateTime.Now;
            Username = username;
        }
    }
}
