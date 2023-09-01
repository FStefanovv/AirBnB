import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateRatingDTO, RatedEntity, RatingDTO, RatingInfoDTO, RatingWithUsernameDTO } from '../model/ratings';
import { ObserversModule } from '@angular/cdk/observers';
import { Observable } from 'rxjs';
import { AccommodationRecommendation } from '../model/accomm-recomm';
import { AuthService } from './auth.service';
import { CanRate } from '../model/canRate';

@Injectable({
  providedIn: 'root'
})
export class RatingServiceService {

  private gatewayUrl = 'https://localhost:5000/gateway/';
  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private authService: AuthService) { }

  rate(dto: CreateRatingDTO, hostId: string, entityName: string) : Observable<CreateRatingDTO> {
    return this.http.post<CreateRatingDTO>(this.gatewayUrl+'rate/'+hostId+'/'+entityName, dto, this.httpOptions);
  }

  getAverageRating(id: string) : Observable<RatedEntity>{
    return this.http.get<RatedEntity>(this.gatewayUrl+'get-average-rating/'+id, this.httpOptions);
  }
  
  getAllRatings(id: string) : Observable<RatingWithUsernameDTO[]> {
    return this.http.get<RatingWithUsernameDTO[]>(this.gatewayUrl+'get-all-ratings/'+id, this.httpOptions);
  }
  
  deleteRating(entityId: string) {
    return this.http.delete<RatingDTO>(this.gatewayUrl+'delete-rating/'+entityId, this.httpOptions);
  }

  getUsersRating(entityId: string) : Observable<RatingDTO> {
    return this.http.get<RatingDTO>(this.gatewayUrl+'get-user-rating/'+entityId, this.httpOptions);
  }

  getPageRatingInfo(accommId: string, hostId: string) : Observable<RatingInfoDTO[]> {
    if(this.authService.isLoggedIn()){
      const userId = this.authService.getId();
      const httpOptionsWithToken = {
        headers: new HttpHeaders({ 
          'Content-Type': 'application/json',
          'UserId': userId
        })
      };
      return this.http.get<RatingInfoDTO[]>(this.gatewayUrl+'get-ratings-for-page/'+accommId+'/'+hostId, httpOptionsWithToken);
    }
    else
    return this.http.get<RatingInfoDTO[]>(this.gatewayUrl+'get-ratings-for-page/'+accommId+'/'+hostId, this.httpOptions);
  }

  getAccommodationRecomemendations() : Observable<AccommodationRecommendation[]> {
    return this.http.get<AccommodationRecommendation[]>(this.gatewayUrl+'get-accommodation-recommendations', this.httpOptions)
  }

  canRate(id: string, hostId: string) :  Observable<CanRate>{
    return this.http.get<CanRate>(this.gatewayUrl+'can-rate/'+id+'/'+hostId, this.httpOptions);
  }
}
