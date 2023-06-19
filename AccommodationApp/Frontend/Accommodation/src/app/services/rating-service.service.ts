import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RatingDTO } from '../model/rated-entity';
import { ObserversModule } from '@angular/cdk/observers';
import { Rating } from '../model/rating';
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

  rate(dto: RatingDTO) {
    this.http.post<RatingDTO>(this.gatewayUrl+'rate', dto, this.httpOptions);
  }

  getAverageRating(id: string) : Observable<Rating>{
    return this.http.get<Rating>(this.gatewayUrl+'get-average-rating/'+id, this.httpOptions);
  }

  getAllRatings(id: string) : Observable<Rating[]> {
    return this.http.get<Rating[]>(this.gatewayUrl+'get-all-ratings/'+id, this.httpOptions);
  }

  deleteRating(entityId: string) : Observable<Rating>{
    return this.http.delete<Rating>(this.gatewayUrl+'delete-rating/'+entityId, this.httpOptions);
  }
}
