using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accommodation.Model
{
    public class Accommodation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string[] Offers { get; set; }
        public int MinGuests { get; set; }
        public int MaxGuests { get; set; }
        public string HostId { get; set; }
        public bool AutoApprove { get; set; }
        public DateTime StartSeasonDate { get; set; }
        public DateTime EndSeasonDate { get; set; }
        public List<DateTime> StartDatesReservations { get; set; }
        public List<DateTime?> EndDatesReservations { get; set; }
        public Price AccomodationPrice { get; set; }

        public Accommodation( string name, Address address, string[] offers, int minGuests, int maxGuests, string hostId, bool autoApprove, DateTime startSeasonDate, DateTime endSeasonDate, List<DateTime> startDatesReservations, List<DateTime?> endDatesReservations, Price accomodationPrice)
        {
     
            Name = name;
            Address = address;
            Offers = offers;
            MinGuests = minGuests;
            MaxGuests = maxGuests;
            HostId = hostId;
            AutoApprove = autoApprove;
            StartSeasonDate = startSeasonDate;
            EndSeasonDate = endSeasonDate;
            StartDatesReservations = startDatesReservations;
            EndDatesReservations = endDatesReservations;
            AccomodationPrice = accomodationPrice;
        }

        public Accommodation()
        {
        }
    }
}
