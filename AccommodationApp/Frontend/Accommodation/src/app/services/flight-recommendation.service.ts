import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FlightRequest } from '../model/flight-requests';
import { ObserversModule } from '@angular/cdk/observers';
import { Observable } from 'rxjs';
import { FlightRecommendation } from '../model/flight-recommendation';
import { FlightTicketDTO } from '../model/buy-flight-ticket';

@Injectable({
  providedIn: 'root'
})
export class FlightRecommendationService {
  
  private gatewayUrl = 'https://localhost:5000/gateway/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getRecommendations(flightRequest: FlightRequest) : Observable<FlightRecommendation[]> {
    return this.http.post<FlightRecommendation[]>(this.gatewayUrl+'get-flight-recommendations', flightRequest, this.httpOptions);
  }

  buyTickets(ticketDTO: FlightTicketDTO) : Observable<FlightTicketDTO> {
    return this.http.post<FlightTicketDTO>(this.gatewayUrl+'purchase-flight-tickets', ticketDTO, this.httpOptions);
  }
}
