import { NumberValueAccessor } from "@angular/forms";

export class Flight {
    id?: string;
    departurePoint?: string;
    arrivalPoint?: string;
    departureTime?: Date;
    duration?: number;
    ticketPrice?: number;
    numberOfPassengers?: number;
    remainingTickets?: number;
    status?:number;
}