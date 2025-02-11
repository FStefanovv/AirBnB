export class Request {
    id?: string;
    from?: Date;
    to?: Date;
    userId?: string;
    accommodationName?: string;
    accommodationId?: string;
    numberOfGuests?: number;
    status?: RequestStatus;
    hostId?: string;

    public constructor(){}
}

export enum RequestStatus {
    PENDING,
    DENIED,
    ACCEPTED,
    CANCELLED,
    USER_DELETED
}