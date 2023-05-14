import { Address } from "./address";

export class SearchDTO {
    location? : Address
    numberOfGuets? : number;
    checkIn? : Date;
    checkOut? : Date;
}