import { Address } from "./address";

export class CreateAccommodationDTO {
    name?: string
    address?: Address;
    offers?: string[];
    minGuests?: number;
    maxGuests?: number;
    autoApprove?: boolean;
    startSeason?: string;
    endSeason?: string;
    price: number;
    pricePerGuest?: boolean;
    pricePerAccomodation?: boolean;
    holidayCost?: boolean;
    weekendCost?: boolean;
    summerCost?: boolean;


    constructor(){
        this.name = '';
        this.address = new Address();
        this.offers = [];
        this.minGuests = 0;
        this.maxGuests = 0;
        this.autoApprove = false;
        this.startSeason = '';
        this.endSeason = '';
        this.price =0;
        this.pricePerGuest=false;
        this.pricePerAccomodation=false;
        this.holidayCost=false;
        this.weekendCost=false;
        this.summerCost=false;
    }
}
