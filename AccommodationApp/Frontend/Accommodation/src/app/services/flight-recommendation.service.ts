import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FlightRequest } from '../model/flight-requests';
import { ObserversModule } from '@angular/cdk/observers';
import { Observable } from 'rxjs';
import { FlightRecommendation } from '../model/flight-recommendation';

@Injectable({
  providedIn: 'root'
})
export class FlightRecommendationService {
  
  private gatewayUrl = 'http://localhost:5000/gateway/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  getRecommendations(flightRequest: FlightRequest) : Observable<FlightRecommendation[]> {
    return this.http.post<FlightRecommendation[]>(this.gatewayUrl+'get-flight-recommendations', flightRequest, this.httpOptions);
  }
}
