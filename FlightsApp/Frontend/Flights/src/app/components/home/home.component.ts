import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Flight } from 'src/app/model/flight';
import { AuthService } from 'src/app/services/auth.service';
import { FlightsService } from 'src/app/services/flights.service';

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
  role: string = "UNREGISTERED";

  ngOnInit(): void {
     this.getAllFlights();
      if(this.authService.isLoggedIn())
        this.role = this.authService.getRole();
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
