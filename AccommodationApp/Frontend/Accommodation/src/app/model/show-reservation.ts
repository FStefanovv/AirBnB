export class ShowReservation {
    id?: string;
    from?: Date;
    to?: Date;
    accommodationName?: string;
    accommodationLocation?: string;
    numberOfGuests?: number;
    status?: ReservationStatus;
    price?: number;
}


export enum ReservationStatus {
    ACTIVE, CANCELLED
}