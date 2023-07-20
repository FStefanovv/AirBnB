import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShowReservation } from '../model/show-reservation';
import { CreateRequestDto } from '../model/createRequestDto';


@Injectable({
  providedIn: 'root'
})
export class ReservationService {

  private gatewayUrl = 'https://localhost:5000/gateway/';
  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getUsersReservations() : Observable<ShowReservation[]> {
    return this.http.get<ShowReservation[]>(this.gatewayUrl+'get-user-reservations', this.httpOptions);
  }

  cancel(id: string) : any {
    return this.http.put<any>(this.gatewayUrl+'cancel-reservation/'+id, this.httpOptions);
  }

  getPendingRequests() : Observable<Request[]> {
    return this.http.get<Request[]>(this.gatewayUrl+'get-pending-requests', this.httpOptions)
  }

  createRequest(dto:CreateRequestDto): Observable<CreateRequestDto>{
    return this.http.post<CreateRequestDto>(this.gatewayUrl+'create-request',this.httpOptions);
  }

  getReservation(id: string) : Observable<ShowReservation> {
    return this.http.get<ShowReservation>(this.gatewayUrl+'get-reservation/'+id,this.httpOptions);
  }

}
