import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';


import { catchError, Observable, throwError } from 'rxjs';
import { Flight } from '../model/flight';
import { SearchedFlightDTO } from '../model/searchedFlightDto';
import { NewFlightDto } from '../model/newFlightDto';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
}) 
export class FlightsService {

  private flightsUrl = 'https://localhost:5010/api/Flights/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json'})
  };

  getHttpHeadersRegular() : HttpHeaders {
    
    return  new HttpHeaders({'Content-Type': 'application/json'})
  }

  getHttpHeadersBearer() : HttpHeaders {
    const token = this.authService.getToken();
    
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization':`Bearer ${token}`});
  }
  
  constructor(private http: HttpClient, private authService: AuthService) { }

  getAll() : Observable<Flight[]> {
    return this.http.get<Flight[]>(this.flightsUrl, this.httpOptions);
  }

  getSearchedFlights(departurePoint : string, arrivalPoint : string, numberOfPassengers : number, departureTime : string) : Observable<Flight[]> {
    return this.http.get<Flight[]>(this.flightsUrl + 'GetSearchedFlights?departurePoint=' + departurePoint +
                                    '&arrivalPoint=' + arrivalPoint +
                                    '&numberOfPassenger=' + numberOfPassengers +
                                    '&dateOfDeparture=' + departureTime,this.httpOptions)
  }

  new(newFlight: NewFlightDto) : Observable<NewFlightDto> {
   
    return this.http.post<NewFlightDto>(this.flightsUrl, newFlight, {headers: this.getHttpHeadersBearer()});
  }

  delete(id: any): Observable<any> {
    return this.http.delete<any>(this.flightsUrl+ 'Delete/' + id,  {headers: this.getHttpHeadersBearer()});
  }
}