import { Component, OnInit,VERSION } from '@angular/core';
import { NewFlightDto } from 'src/app/model/newFlightDto';
import { FlightsService } from 'src/app/services/flights.service';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-new-flight',
  templateUrl: './new-flight.component.html',
  styleUrls: ['./new-flight.component.css']
})
export class NewFlightComponent implements OnInit {

  public newFlight: NewFlightDto = new NewFlightDto ()
  public dateNow: any
  public defaultDate: any
  public departurePointValid: boolean = true
  public arrivalPointValid: boolean = true
  public reamainingTicketValid: boolean = true
  public numberOfPassengerValid: boolean = true
  public ticketPriceValid: boolean = true
  public durationValid: boolean = true
  public dateValid:boolean =true
    
  
  
 

  constructor(private flightService: FlightsService,private date: DatePipe) { }

  ngOnInit(): void {    
  this.dateNow=this.date.transform((new Date),"yyyy-MM-ddTHH:mm")
  this.defaultDate=this.date.transform((new Date().setDate(this.dateNow)),"yyyy-MM-ddTHH:mm" )
  console.log(this.dateNow)

  }

  
  private isArrivalPointValid(): boolean{
    this.arrivalPointValid=true;
    return (this.newFlight.arrivalPoint != undefined && this.newFlight.arrivalPoint != '')
  }
  private isDeparturePointValid(): boolean {
    this.departurePointValid=true;
    return (this.newFlight.departurePoint != undefined && this.newFlight.departurePoint != '')
  }
  private isDurationValid(): boolean{
    this.durationValid=true;
    return this.newFlight.duration > 0
  }
  private isRemainingTicketValid(): boolean{
    this.reamainingTicketValid=true;
    return this.newFlight.remainingTickets >= 0
  }
  private isNumberOfPassengerValid(): boolean{
    this.numberOfPassengerValid=true
    return this.newFlight.numberOfPassengers >= 0
  }
  private isTicketPriceValid(): boolean{
    this.ticketPriceValid=true
    return this.newFlight.ticketPrice > 0
  }
  private isDateValid(): boolean {
    this.dateValid=true;
    return (this.newFlight.departureTime != undefined && this.newFlight.departureTime != '')
  }


  
  public newFlightSubmit(){

    console.log("date: ",this.newFlight.departureTime)
    console.log(this.newFlight.departurePoint)
    console.log(this.newFlight.remainingTickets)
    console.log(this.newFlight.departureTime)
    console.log("arrivalPoint" ,this.newFlight.arrivalPoint)
    let provera= this.newFlight.arrivalPoint != undefined
    console.log("provera: ",provera)
    console.log("arrivalValid: ",this.arrivalPointValid)

    console.log("isArrivalValid(): ", this.isArrivalPointValid())
    console.log("isDepartureValid(): ", this.isDeparturePointValid())
    console.log("isDurationValid(): ", this.isDurationValid())
    console.log("isRemainingTicketValid(): ", this.isRemainingTicketValid())
    console.log("isNumberOfPassengerValid(): ", this.isNumberOfPassengerValid())
    console.log("isTicketPriceValid(): ", this.isTicketPriceValid())

    
    if(!this.isArrivalPointValid()) this.arrivalPointValid=false
    console.log("arrivalValid: ",this.arrivalPointValid)
    if(!this.isDeparturePointValid()) this.departurePointValid=false
    console.log("departureValid: ",this.departurePointValid)
    if(!this.isDurationValid()) this.durationValid=false
    console.log("durationValid: ",this.durationValid)
    if(!this.isRemainingTicketValid()) this.reamainingTicketValid=false
    console.log("reamainingTicketValid: ",this.reamainingTicketValid)
    if(!this.isNumberOfPassengerValid()) this.numberOfPassengerValid=false
    console.log("numberOfPassengerValid: ",this.numberOfPassengerValid)
    if(!this.isTicketPriceValid()) this.ticketPriceValid=false
    console.log("ticketPriceValid: ",this.ticketPriceValid)
    if(!this.isDateValid()) this.dateValid=false
    console.log("dateValid: ",this.dateValid)
    if(this.arrivalPointValid==false || this.departurePointValid==false || this.durationValid==false || this.reamainingTicketValid==false || this.numberOfPassengerValid==false || this.ticketPriceValid==false || this.dateValid==false) return alert("You must correctly fill all fields")
    this.flightService.new(this.newFlight).subscribe((res: any)=>{
      alert("Successfully!")
      },
      (error:any)=>{alert("Bad request!")})
        
      
  }

 

}
