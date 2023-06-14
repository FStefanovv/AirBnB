export class UpdateAccommodation{
    id?:string
    startSeason?:string
    endSeason?:string
    price?:number;

    constructor(){
        this.id='';
        this.startSeason='';
        this.endSeason='';
        this.price=0;
    }
}