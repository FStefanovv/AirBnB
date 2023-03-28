import { NumberValueAccessor } from "@angular/forms";

export class Flight {
    departurePoint?: string;
    arrivalPoint?: string;
    departureTime?: Date;
    duration?: number;
    ticketPrice?: number;
    numberOfPassengers?: number;
    remainingTickets?: number;
    status?:number;
}