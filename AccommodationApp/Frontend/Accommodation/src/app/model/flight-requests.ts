export class FlightRequest {
    airportLocation: string;
    departureDate: Date;
    accommodationLocation: string;
    direction: number;

    public constructor(){
        this.airportLocation = "";
        this.departureDate = new Date();
        this.accommodationLocation = "";
        this.direction = 1;
    }
}