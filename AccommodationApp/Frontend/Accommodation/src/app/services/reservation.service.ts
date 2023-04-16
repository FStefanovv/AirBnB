import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Reservation } from '../model/reservation';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  private gatewayUrl = 'http://localhost:5000/gateway/';
  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getUsersReservations() : Observable<Reservation[]> {
    return this.http.get<Reservation[]>(this.gatewayUrl+'get-user-reservations', this.httpOptions);
  }

  cancel(id: string) : any {
    return this.http.put<any>(this.gatewayUrl+'cancel-reservation/'+id, this.httpOptions);
  }
}
