import { Address } from "./address";

export class AccommodationDTO {
    id?:string;
    name?: string
    startSeason?: string;
    endSeason?: string;
    price?: number;
    pricePerGuest?: boolean;
    pricePerAccomodation?: boolean;
    holidayCost?: boolean;
    weekendCost?: boolean;
    summerCost?: boolean;
    isDistinguishedHost?: boolean;
    offers?: string[];
    hostId? : string;
    hostUsername?: string;
    accommodationLocation? : string;

    constructor(){
        this.id='';
        this.name = '';
        this.startSeason = '';
        this.endSeason = '';
        this.price = 0;
        this.pricePerGuest = false;
        this.pricePerAccomodation = false;
        this.holidayCost = false;
        this.weekendCost = false;
        this.summerCost = false;
        this.isDistinguishedHost = false;
        this.offers = [];
        this.hostId = '';
        this.hostUsername = '';
        this.accommodationLocation = "";
    }
}