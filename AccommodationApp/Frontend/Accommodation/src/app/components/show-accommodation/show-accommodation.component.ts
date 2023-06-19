import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccommodationDTO } from 'src/app/model/accommodation';
import { RatedEntity, RatingDTO } from 'src/app/model/rated-entity';
import { Rating } from 'src/app/model/rating';
import { AccommodationService } from 'src/app/services/accommodation.service';
import { RatingServiceService } from 'src/app/services/rating-service.service';

@Component({
  selector: 'app-show-accommodation',
  templateUrl: './show-accommodation.component.html',
  styleUrls: ['./show-accommodation.component.css']
})
export class ShowAccommodationComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private accommodationService:AccommodationService, private ratingService: RatingServiceService) { }

  accommodationId: string = '';
  accommodation: AccommodationDTO = new AccommodationDTO();

  ratingDto: RatingDTO = new RatingDTO();
  
  accommRating: RatedEntity = new RatedEntity();
  hostRating: RatedEntity = new RatedEntity();

  hostRatingInfo: string = '';
  accommRatingInfo: string = '';
  offers: string = '';

  ngOnInit(): void {
    const temp = this.activatedRoute.snapshot.paramMap.get("id");
    if(temp)
      this.accommodationId = temp;
    this.accommodationService.getAccommodation(this.accommodationId).subscribe(
      res =>
      {
        this.accommodation = res;
      }
    );
    if(this.accommodationId)
      this.ratingService.getAverageRating(this.accommodationId).subscribe({ 
        next: (res: RatedEntity) => {
          this.hostRatingInfo = 'Host average rating is '+ res.AverageRating;
        },
        error: (error: HttpErrorResponse) => {
          this.hostRatingInfo = 'Host not rated yet';
        }
        });
    if(this.accommodation.hostId){
      this.ratingService.getAverageRating(this.accommodation.hostId).subscribe({ 
        next: (res: RatedEntity) => {
          this.accommRatingInfo = 'Accommodation average rating is '+ res.AverageRating;
        },
        error: (error: HttpErrorResponse) => {
          this.hostRatingInfo = 'Accommodation not rated yet';
        }
        });
    }
  }

  rateAccomm(){
    this.ratingDto.RatedEntityId = this.accommodationId;
    this.ratingService.rate(this.ratingDto);
  }

  rateHost(){
    if(this.accommodation.hostId){
      this.ratingDto.RatedEntityId = this.accommodation.hostId;
    }
    this.ratingService.rate(this.ratingDto);
  }

  deleteAccommRating() {
    this.ratingService.deleteRating(this.accommodationId).subscribe();
  }

  deleteHostRating(){
    if(this.accommodation.hostId)
      this.ratingService.deleteRating(this.accommodation.hostId).subscribe();
  }
}
