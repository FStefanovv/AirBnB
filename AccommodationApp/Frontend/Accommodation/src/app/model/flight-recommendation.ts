export class FlightRecommendation {
    flightId: string;
    departureTime: Date;
    duration: number;
    ticketPrice: number;

    constructor() {
        this.flightId = "";
        this.departureTime = new Date();
        this.duration = 0;
        this.ticketPrice = 0.0;
    }
}