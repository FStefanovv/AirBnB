export class CreateRequestDto{
  accomodationId?: string;
  startDate?: string;
  endDate?: string;
  numberOfGuests?: number;
  hostId?: string;
  userId?: string;
  accommodationName?: string;
  accommodationLocation?: string;
  price?: number;

  constructor(){
    this.accomodationId="";
    this.startDate = "";
    this.endDate = "";
    this.numberOfGuests = 0;
    this.hostId = "";
    this.userId = "";
    this.accommodationName = ""
    this.accommodationLocation = ""
    this.price = 0;
  }
}