import { Component, OnInit } from '@angular/core';
import { BuyTicketApiKeyDto } from 'src/app/model/buyWithApiKey';
import { Flight } from 'src/app/model/flight';
import { FlightsService } from 'src/app/services/flights.service';

@Component({
  selector: 'app-purchase-tickets-api-key',
  templateUrl: './purchase-tickets-api-key.component.html',
  styleUrls: ['./purchase-tickets-api-key.component.css']
})
export class PurchaseTicketsApiKeyComponent implements OnInit {

  constructor(private flightService: FlightsService) { }

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
    console.log(this.purchaseInfo);
  }

}
