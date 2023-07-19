namespace Accommodation.Model
{
    public class Price
    {
        public double FinalPrice { get; set; }
        public bool PricePerGuest { get; set; }

        public bool PricePerAccomodation{ get; set; }

        public bool WeekendCost { get; set; }

        public bool SummerCost { get; set; }

        public bool HolidayCost { get; set; }

        public Price(double finalPrice, bool pricePerGuest, bool pricePerAccomodation, bool isWeekend, bool isSummer, bool isHoliday)
        {
            FinalPrice = finalPrice;
            PricePerGuest = pricePerGuest;
            PricePerAccomodation = pricePerAccomodation;
            WeekendCost = isWeekend;
            SummerCost = isSummer;
            HolidayCost = isHoliday;
        }

        public Price()
        {
        }
    }
}