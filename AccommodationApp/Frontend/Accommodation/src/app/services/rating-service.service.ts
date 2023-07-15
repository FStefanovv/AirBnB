import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateRatingDTO, RatedEntity, RatingDTO } from '../model/ratings';
import { ObserversModule } from '@angular/cdk/observers';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RatingServiceService {

  private gatewayUrl = 'http://localhost:5000/gateway/';
  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  rate(dto: CreateRatingDTO) {
    this.http.post<RatingDTO>(this.gatewayUrl+'rate', dto, this.httpOptions);
  }

  getAverageRating(id: string) : Observable<RatedEntity>{
    return this.http.get<RatedEntity>(this.gatewayUrl+'get-average-rating/'+id, this.httpOptions);
  }
  
  getAllRatings(id: string) : Observable<RatingDTO[]> {
    return this.http.get<RatingDTO[]>(this.gatewayUrl+'get-all-ratings/'+id, this.httpOptions);
  }
  
  deleteRating(entityId: string) {
    return this.http.delete<RatingDTO>(this.gatewayUrl+'delete-rating/'+entityId, this.httpOptions);
  }

  getUsersRating(entityId: string) : Observable<RatingDTO> {
    return this.http.get<RatingDTO>(this.gatewayUrl+'get-user-rating/'+entityId, this.httpOptions);
  }
}
