export class Reservation {
    id?: string;
    from?: Date;
    to?: Date;
    userId?: string;
    accommodationName?: string;
    accommodationId?: string;
    numberOfGuests?: number;
    status?: ReservationStatus;
}

export enum ReservationStatus {
    ACTIVE, CANCELLED
}