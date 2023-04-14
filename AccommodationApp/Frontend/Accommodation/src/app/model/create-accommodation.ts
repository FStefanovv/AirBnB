import { Address } from "./address";

export class CreateAccommodationDTO {
    name?: string
    address?: Address;
    offers?: string[];
    minGuests?: number;
    maxGuests?: number;

    constructor(){
        this.name = '';
        this.address = new Address();
        this.offers = [];
        this.minGuests = 0;
        this.maxGuests = 0;
    }
}
