export class BuyTicketDto{
  userId?: string
  flightId?: string
  numberOfTickets?: string
  price?: number
  public constructor(obj?:any){
    if(obj){
      this.userId = obj.userId
      this.flightId = obj.flightId
      this.numberOfTickets = obj.numberOfTickets
      this.price = obj.price
    }
  }
}