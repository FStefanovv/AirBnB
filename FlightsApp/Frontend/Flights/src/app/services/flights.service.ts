import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';


import { catchError, Observable, throwError } from 'rxjs';
import { Flight } from '../model/flight';
import { SearchedFlightDTO } from '../model/searchedFlightDto';



@Injectable({
  providedIn: 'root'
})
export class FlightsService {

  private flightsUrl = 'http://localhost:5000/api/Flights/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  };

  
  constructor(private http: HttpClient) { }

  getAll() : Observable<Flight[]> {

    return this.http.get<Flight[]>(this.flightsUrl, this.httpOptions);
  }

  getSearchedFlights(flight : SearchedFlightDTO) : Observable<Flight[]> {
    return this.http.get<Flight[]>(this.flightsUrl,this.httpOptions)
  }

  
}