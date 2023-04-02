export class ViewTicketDto{
  Id?:string
  UserId?:number
  Quantity?:number
  SummedPrice?:string
  DeparturePoint?:string
  ArrivalPoint?:string
  DepartureTime?:string
  Duration?:number
  Valid?:boolean
  public constructor(obj?:any){
    if(obj){
      this.Id = obj.id
      this.UserId = obj.userId
      this.Quantity = obj.quantity
      this.SummedPrice = obj.summedPrice
      this.DeparturePoint = obj.departurePoint
      this.ArrivalPoint = obj.arrivalPoint
      this.DepartureTime = obj.departureTime
      this.Duration = obj.duration
      this.Valid = obj.valid;
    }
  }
}