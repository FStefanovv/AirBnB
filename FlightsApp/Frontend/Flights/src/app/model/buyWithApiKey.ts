export class BuyTicketApiKeyDto {
    flightId: string = '';
    numberOfTickets: number = 0;
}

export class KeyValidUntilDto {
    validUntil: Date = new Date();
    userId: string = '';
}

export class ApiKeyDto {
    id: string = '';
    userId: string = '';
    validUntil: Date = new Date();
}