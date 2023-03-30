import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';


import { catchError, Observable, throwError } from 'rxjs';
import { Flight } from '../model/flight';
import { NewFlightDto } from '../model/newFlightDto';



@Injectable({
  providedIn: 'root'
})
export class FlightsService {

  private flightsUrl = 'http://localhost:5000/api/Flights/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  
  constructor(private http: HttpClient) { }

  getAll() : Observable<Flight[]> {

    return this.http.get<Flight[]>(this.flightsUrl, this.httpOptions);
  }

  new(newFlight: NewFlightDto) : Observable<NewFlightDto> {
    console.log(newFlight.departurePoint)
    console.log(newFlight.remainingTickets)
    console.log(newFlight.departureTime)
    console.log(newFlight)
    return this.http.post<NewFlightDto>(this.flightsUrl, newFlight, this.httpOptions);
  }

  
}