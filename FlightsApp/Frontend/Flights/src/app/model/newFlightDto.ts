export class NewFlightDto {
    
    departurePoint?: string;
    arrivalPoint?:  string;
    departureTime?: string;
    duration: number=0;
    ticketPrice: number=0;
    numberOfPassengers: number=0;
    remainingTickets: number=0;

    public constructor(obj?: any){
        if(obj){
            this.departurePoint = obj.DeparturePoint;
            this.arrivalPoint = obj.ArrivalPoint;
            this.departureTime = obj.DepartureTime;
            this.duration = obj.Duration;
            this.ticketPrice = obj.TicketPrice;
            this.numberOfPassengers = obj.NumberOfPassengers;
            this.remainingTickets = obj.RemainingTickets;
        }
    }

}