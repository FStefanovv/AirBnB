export class ShowRequest {
    requestId?: string;
    from?: Date;
    to?: Date;
    userId?: string;
    accommodationId?: string;
    accommodationName?:string
    hostId?: string;
    numberOfGuests?: number;

    public constructor(){}
}
