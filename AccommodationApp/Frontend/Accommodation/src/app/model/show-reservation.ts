export class ShowReservation {
    id?: string;
    from?: Date;
    to?: Date;
    accommodationName?: string;
    accommodationLocation?: string;
    hostId?: string;
    numberOfGuests?: number;
    status?: ReservationStatus;
    price?: number;

    public constructor(){
        
    }
}


export enum ReservationStatus {
    ACTIVE, PAST, CANCELLED
}