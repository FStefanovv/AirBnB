import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FlightRecommendation } from 'src/app/model/flight-recommendation';
import { FlightRequest } from 'src/app/model/flight-requests';
import { formatDate } from 'src/app/functions/format-date';
import { Router } from '@angular/router';

@Component({
  selector: 'app-display-recommendations',
  templateUrl: './display-recommendations.component.html',
  styleUrls: ['./display-recommendations.component.css']
})
export class DisplayRecommendationsComponent implements OnInit {
  @Input() flightRequest: FlightRequest = new FlightRequest();
  @Input() recommendations: FlightRecommendation[] = [];

  from: string = '';
  to: string = ';';

  formatDate = formatDate;

  displayDialog: boolean = false;

  constructor(private router: Router) { }
        
  ngOnInit(): void {
    if(this.flightRequest.direction==1){
      this.from = this.flightRequest.airportLocation;
      this.to = this.flightRequest.accommodationLocation;
    }
    else{
      this.from = this.flightRequest.accommodationLocation;
      this.to = this.flightRequest.airportLocation;
    }
  }

  buyTickets(recommendation: FlightRecommendation) {
    this.router.navigate(['buy-flight-tickets'], {queryParams: {data: JSON.stringify(recommendation)}});
  }

}
