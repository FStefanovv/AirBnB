import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Flight } from 'src/app/model/flight';
import { SearchedFlightDTO } from 'src/app/model/searchedFlightDto';
import { FlightsService } from 'src/app/services/flights.service';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private flightService:FlightsService, private router: Router) { }

  loggedIn?: boolean;
  allFlights: Flight[] = [];
  flightsToShow: Flight[] = [];
  searchedFlight : SearchedFlightDTO = new SearchedFlightDTO;

  ngOnInit(): void {
     this.getAllFlights();
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

  getSearchedFlights(searchedFlight: SearchedFlightDTO){
    this.flightService.getSearchedFlights(searchedFlight).subscribe({
      next: (response : Flight[]) => {
        this.flightsToShow = response
        console.log(this.flightsToShow)
      },
      error: (error : HttpErrorResponse) => {
        console.log(error.message)
      }
    });
  }

}
