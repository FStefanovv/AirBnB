export class Address {
    city?: string;
    country?: string;
    street?: string;
    number?: number;

    constructor(){
        this.city = "";
        this.country = "";
        this.street = "";
        this.number = 0;
    }
}