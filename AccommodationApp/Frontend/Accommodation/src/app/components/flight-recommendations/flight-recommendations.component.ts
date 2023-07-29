import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FlightRecommendation } from 'src/app/model/flight-recommendation';
import { FlightRequest } from 'src/app/model/flight-requests';
import { ShowReservation } from 'src/app/model/show-reservation';
import { FlightRecommendationService } from 'src/app/services/flight-recommendation.service';
import { ReservationService } from 'src/app/services/reservation.service';
import { formatDate } from 'src/app/functions/format-date';

@Component({
  selector: 'app-flight-recommendations',
  templateUrl: './flight-recommendations.component.html',
  styleUrls: ['./flight-recommendations.component.css']
})
export class FlightRecommendationsComponent implements OnInit {

  reservation: ShowReservation = new ShowReservation();

  arrivalFlightDeparturePoint: string = '';
  departureFlightArrivalPoint: string = '';

  arrivalRecommendations: FlightRecommendation[] = [];
  departureRecommendations: FlightRecommendation[] = [];

  arrivalFlight: FlightRequest = new FlightRequest();
  departureFlight: FlightRequest = new FlightRequest();

  arrivalRecommendationsObtained: boolean = false;
  departureRecommendationsObtained: boolean = false;

  formatDate = formatDate;

  constructor(private activatedRoute : ActivatedRoute, private reservationService : ReservationService, 
    private datePipe: DatePipe, private flightRecommendationService: FlightRecommendationService) { }

  ngOnInit(): void {
    const reservationId = this.activatedRoute.snapshot.paramMap.get("id");
    if(reservationId){
      this.reservationService.getReservation(reservationId).subscribe(
        res => {
          this.reservation = res;
      });
    }
  }
 

  getRecommendations(){
    this.arrivalFlight = this.generateFlightRequest(1);
    this.departureFlight = this.generateFlightRequest(-1);
    
    this.flightRecommendationService.getRecommendations(this.arrivalFlight).subscribe({
      next: (res) =>{
        this.arrivalRecommendations = res;
        this.arrivalRecommendationsObtained = true;
      },
      error: (error: HttpErrorResponse) => {
        console.log(error.message);
      }
    });
    this.flightRecommendationService.getRecommendations(this.departureFlight).subscribe({
      next: (res) =>{
        this.departureRecommendations = res;
        this.departureRecommendationsObtained = true;
      },
      error: (error: HttpErrorResponse) => {
        console.log(error.message);
      }
    });
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

  resetRecommendations() : void {
    this.arrivalRecommendationsObtained = false;
    this.departureRecommendationsObtained = false;
  }
}
