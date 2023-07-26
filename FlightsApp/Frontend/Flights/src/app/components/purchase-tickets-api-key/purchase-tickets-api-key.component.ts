import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { BuyTicketApiKeyDto } from 'src/app/model/buyWithApiKey';
import { Flight } from 'src/app/model/flight';
import { FlightsService } from 'src/app/services/flights.service';
import { TicketsService } from 'src/app/services/tickets.service';

@Component({
  selector: 'app-purchase-tickets-api-key',
  templateUrl: './purchase-tickets-api-key.component.html',
  styleUrls: ['./purchase-tickets-api-key.component.css']
})
export class PurchaseTicketsApiKeyComponent implements OnInit {

  constructor(private flightService: FlightsService, private ticketService: TicketsService) { }

  flights: Flight[] = [];

  key: string = '';

  flightChosen: boolean = false;

  selectedFlight: string = '';

  purchaseInfo: BuyTicketApiKeyDto = new BuyTicketApiKeyDto();

  ngOnInit(): void {
    this.flightService.getAll().subscribe(res =>{
      this.flights = res
    })
  }

  selectFlight(flightId: string | undefined) : void {
    if(flightId){
      this.selectedFlight = flightId;
      this.flightChosen = true;
    }
  }

  purchase(){
    this.purchaseInfo.flightId = this.selectedFlight;
    //console.log(this.purchaseInfo);
    this.ticketService.purchaseTicketApikey(this.purchaseInfo, this.key).subscribe({
      next: (response: any) => {
        console.log('successfully purchased');
      },
      error : (err: HttpErrorResponse) => {
        console.log(err.error);
      }
    });
  }

}
