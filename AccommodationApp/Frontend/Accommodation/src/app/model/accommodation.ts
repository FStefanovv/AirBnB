import { Address } from "./address";

export class AccommodationDTO {
    name?: string
    startSeason?: string;
    endSeason?: string;
    price?: number;
    pricePerGuest?: boolean;
    pricePerAccomodation?: boolean;
    holidayCost?: boolean;
    weekendCost?: boolean;
    summerCost?: boolean;

    constructor(){
        this.name = '';
        this.startSeason = '';
        this.endSeason = '';
        this.price = 0;
        this.pricePerGuest = false;
        this.pricePerAccomodation = false;
        this.holidayCost = false;
        this.weekendCost = false;
        this.summerCost = false;
    }
}