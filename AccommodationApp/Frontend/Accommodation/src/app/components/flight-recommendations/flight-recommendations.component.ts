import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FlightRequest } from 'src/app/model/flight-requests';
import { ShowReservation } from 'src/app/model/show-reservation';
import { FlightRecommendationService } from 'src/app/services/flight-recommendation.service';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-flight-recommendations',
  templateUrl: './flight-recommendations.component.html',
  styleUrls: ['./flight-recommendations.component.css']
})
export class FlightRecommendationsComponent implements OnInit {

  reservation: ShowReservation = new ShowReservation();
  arrivalFlightDeparturePoint: string = '';
  departureFlightArrivalPoint: string = '';


  constructor(private activatedRoute : ActivatedRoute, private reservationService : ReservationService, 
    private datePipe: DatePipe, private flightRecommendationService: FlightRecommendationService) { }

  ngOnInit(): void {
    const reservationId = this.activatedRoute.snapshot.paramMap.get("id");
    if(reservationId){
      this.reservationService.getReservation(reservationId).subscribe(
        res => {
          this.reservation = res;
          console.log(this.reservation);
      });
    }
  }

  formatReservationDate(date: any): any {
    const dateTransformed = this.datePipe.transform(date, 'dd-MMM-yyyy');
    if(dateTransformed)
      return dateTransformed;
  }
  

  getRecommendations(){
    const arrivalFlight: FlightRequest = this.generateFlightRequest(1);
    const departureFlight: FlightRequest = this.generateFlightRequest(-1);
    
    this.flightRecommendationService.getRecommendations(arrivalFlight).subscribe(
      res =>{
        console.log(res)
      }
    );
    this.flightRecommendationService.getRecommendations(departureFlight).subscribe(
      res =>{
        console.log(res)
      }
    );
  }

  generateFlightRequest(direction: number) : FlightRequest {
    let flightRequest: FlightRequest = new FlightRequest();
    if(direction==1){
      flightRequest.airportLocation = this.arrivalFlightDeparturePoint;
      if(this.reservation.from)
        flightRequest.departureDate = this.reservation.from;
      if(this.reservation.accommodationLocation)
        flightRequest.accommodationLocation = this.reservation.accommodationLocation;
      flightRequest.direction = 1;
    }
    else {
      flightRequest.airportLocation = this.departureFlightArrivalPoint;
      if(this.reservation.to)
        flightRequest.departureDate = this.reservation.to;
      if(this.reservation.accommodationLocation)
        flightRequest.accommodationLocation = this.reservation.accommodationLocation;
      flightRequest.direction = -1;
    }
    return flightRequest;
  }
 


}
