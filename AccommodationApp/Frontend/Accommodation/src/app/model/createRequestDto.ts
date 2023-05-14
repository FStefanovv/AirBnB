export class CreateRequestDto{
  accomodationId?: string;
  startDate?: string;
  endDate?: string;
  numberOfGuest?: number;
  hostId?: string;
  userId?: string;

  constructor(){
    this.accomodationId="";
    this.startDate = "";
    this.endDate = "";
    this.numberOfGuest = 0;
    this.hostId = "";
    this.userId = "";
  }
}