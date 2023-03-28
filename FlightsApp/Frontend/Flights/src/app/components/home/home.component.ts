import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Flight } from 'src/app/model/flight';
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

}
