import { TicketsService } from './../../services/tickets.service';
import { AuthService } from 'src/app/services/auth.service';
import { BuyTicketDto } from './../../model/buyTicketDto';
import { Component, OnInit } from '@angular/core';
import { Flight } from 'src/app/model/flight';
import { FlightsService } from 'src/app/services/flights.service';
import { Router } from '@angular/router';

@Component({
  selector: 'purchase-tickets-regular',
  templateUrl: './purchase-tickets.component.html',
  styleUrls: ['./purchase-tickets.component.css']
})
export class PurchaseTicketsComponent implements OnInit {
  public flights: Flight[] | undefined
  constructor(private flightService: FlightsService,private authService:AuthService,private ticketService:TicketsService,private router: Router) { }
  
  ngOnInit(): void {
    this.flightService.getAll().subscribe(res =>{
      this.flights = res
    })
  }

  bouTicket(flighId:string|undefined,price: number|undefined,numberOfTickets:number){
    let dto = new BuyTicketDto()
    dto.flightId = flighId
    dto.numberOfTickets = numberOfTickets.toString()
    dto.userId = this.authService.getId()
    dto.price= price
    this.ticketService.buyTicket(dto).subscribe(res=>{
      console.log(res)
      this.router.navigate(['view-bought-tickets'])
    })
  }


}
