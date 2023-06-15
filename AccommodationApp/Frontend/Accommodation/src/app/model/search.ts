import { Address } from "./address";

export class SearchDTO {
    location? : String;
    numberOfGuets? : number;
    checkIn? : String;
    checkOut? : String;

    constructor(){
        this.location = '';
        this.numberOfGuets = 0
        this.checkIn = ''
        this.checkOut = ''
    }
}