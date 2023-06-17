import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FlightRecommendation } from 'src/app/model/flight-recommendation';
import { formatDate } from 'src/app/functions/format-date';
import { FlightTicketDTO } from 'src/app/model/buy-flight-ticket';
import { FlightRecommendationService } from 'src/app/services/flight-recommendation.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-buy-flight-tickets',
  templateUrl: './buy-flight-tickets.component.html',
  styleUrls: ['./buy-flight-tickets.component.css']
})
export class BuyFlightTicketsComponent implements OnInit {

  recommendation: FlightRecommendation = new FlightRecommendation();

  formatDate = formatDate;

  ticketDTO: FlightTicketDTO = new FlightTicketDTO();

  attemptedPurchase: boolean = false;

  message : string = '';

  constructor(private route: ActivatedRoute, private flightRecommendationService: FlightRecommendationService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const data = params['data'];
      this.recommendation = JSON.parse(data);
    });
    this.ticketDTO.flightId = this.recommendation.flightId;
  }

  buyTickets() {

    this.flightRecommendationService.buyTickets(this.ticketDTO).subscribe({
      next: (res: any) => {
        this.attemptedPurchase = true;
        this.message = 'Purchased successfully'
      },
      error: (error: HttpErrorResponse) => {
        this.attemptedPurchase = true;
        this.message = error.error;
      } 
    });
  }



}
