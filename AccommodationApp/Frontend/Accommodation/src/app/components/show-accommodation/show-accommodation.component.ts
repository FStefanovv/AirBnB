import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccommodationDTO } from 'src/app/model/accommodation';
import { CreateRatingDTO, RatedEntity, RatingDTO } from 'src/app/model/ratings';
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

  createAccommodationRatingDto: CreateRatingDTO = new CreateRatingDTO();
  createHostRatingDto: CreateRatingDTO = new CreateRatingDTO();
  
  accommRating: RatedEntity = new RatedEntity();
  hostRating: RatedEntity = new RatedEntity();

  hostRatingInfo: string = '';
  accommRatingInfo: string = '';

  userHostRatingInfo: string = '';
  userAccommRatingInfo: string = '';


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
    if(this.accommodationId){
      this.ratingService.getAverageRating(this.accommodationId).subscribe({ 
        next: (res: any) => {
          this.hostRatingInfo = 'Host average rating is '+ res.AverageRating;
        },
        error: (error: HttpErrorResponse) => {
          this.hostRatingInfo = 'The accommodation doesn\'t have any ratings yet';
        }
        });
      this.ratingService.getUsersRating(this.accommodationId).subscribe({
        next: (res: RatingDTO) => {
            
        },
        error: (error: HttpErrorResponse) => {
          
        }
        });
    }
    if(this.accommodation.hostId){
      this.ratingService.getAverageRating(this.accommodation.hostId).subscribe({ 
        next: (res: any) => {
          this.accommRatingInfo = 'Accommodation average rating is '+ res.AverageRating;
        },
        error: (error: HttpErrorResponse) => {
          this.hostRatingInfo = 'The host doesn\'t have any ratings yet';
        }
        });
     this.ratingService.getUsersRating(this.accommodation.hostId).subscribe({
      next: (res: RatingDTO) => {
        
      },
      error: (error: HttpErrorResponse) => {

      }
     });
    }
  }

  rateAccomm(){
    this.createAccommodationRatingDto.RatedEntityId = this.accommodationId;
    this.createAccommodationRatingDto.RatedEntityType = 0;
    this.ratingService.rate(this.createAccommodationRatingDto);
  }

  rateHost(){
    if(this.accommodation.hostId){
      this.createHostRatingDto.RatedEntityId = this.accommodation.hostId;
      this.createHostRatingDto.RatedEntityType = 1;
    }
    this.ratingService.rate(this.createHostRatingDto);
  }

  deleteAccommRating() {
    this.ratingService.deleteRating(this.accommodationId).subscribe();
  }

  deleteHostRating(){
    if(this.accommodation.hostId)
      this.ratingService.deleteRating(this.accommodation.hostId).subscribe();
  }
}
