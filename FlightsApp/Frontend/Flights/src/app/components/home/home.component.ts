import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Flight } from 'src/app/model/flight';
import { SearchedFlightDTO } from 'src/app/model/searchedFlightDto';
import { AuthService } from 'src/app/services/auth.service';
import { FlightsService } from 'src/app/services/flights.service';
import { FlightCardComponent } from '../flight-card/flight-card.component';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private flightService:FlightsService, private authService: AuthService, private router: Router) { }

  loggedIn?: boolean;
  allFlights: Flight[] = [];
  flightsToShow: Flight[] = [];
  searchedFlight : SearchedFlightDTO = new SearchedFlightDTO();
  role: string = "UNREGISTERED";

  ngOnInit(): void {
    this.getAllFlights();
    if(this.authService.isLoggedIn()){
      this.role = this.authService.getRole();
    }
  }

  getAllFlights() {
    this.flightService.getAll().subscribe({
      next: (response: Flight[]) => {
        this.allFlights = response;
        this.flightsToShow = response;
      },
      error: (error: HttpErrorResponse) => {
        console.log(error.message);
      }
    });
  }

  getSearchedFlights(){
    if(this.searchedFlight.arrivalPoint=='' || this.searchedFlight.departurePoint=='' || 
    this.searchedFlight.departureTime=='')
      return;
    this.flightService.getSearchedFlights(this.searchedFlight.departurePoint,this.searchedFlight.arrivalPoint,this.searchedFlight.numberOfPassengers,this.searchedFlight.departureTime).subscribe({
      next: (response : Flight[]) => {
        this.flightsToShow = response
      },
      error: (error : HttpErrorResponse) => {
        console.log(error.message)
      }
    });
  }

  cancelFlight(id: string) {
    this.flightService.delete(id).subscribe(res=>{
      window.location.reload();
    },error=>{
      console.log(error)
    });
    
  }

  resetSearch() {
    this.getAllFlights();
    this.searchedFlight.arrivalPoint = '';
    this.searchedFlight.departurePoint = '';
    this.searchedFlight.departureTime = '';
    this.searchedFlight.numberOfPassengers = 1;
  }

}
