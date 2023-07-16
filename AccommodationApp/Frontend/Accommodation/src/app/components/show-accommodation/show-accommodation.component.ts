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

  existingAccommRating: RatingDTO = new RatingDTO();
  existingHostRating: RatingDTO = new RatingDTO();
  
  accommRating: RatedEntity = new RatedEntity();
  hostRating: RatedEntity = new RatedEntity();

  hasRatedAccomm: boolean = false;
  hasRatedHost: boolean = false;

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
        this.obtainAccommAverageRatingInfo();
      }
    );
  }

  rateAccomm(){
    this.createAccommodationRatingDto.ratedEntityId = this.accommodationId;
    this.createAccommodationRatingDto.ratedEntityType = 0;
    this.ratingService.rate(this.createAccommodationRatingDto);
  }

  rateHost(){
    if(this.accommodation.hostId){
      this.createHostRatingDto.ratedEntityId = this.accommodation.hostId;
      this.createHostRatingDto.ratedEntityType = 1;
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

  obtainAccommAverageRatingInfo() {
    if(this.accommodation.id){
      this.ratingService.getAverageRating(this.accommodation.id).subscribe({ 
        next: (res: RatedEntity) => {
          this.accommRatingInfo = 'Average accommodation rating is '+ res.averageRating;
        },
        error: (error: HttpErrorResponse) => {
          this.accommRatingInfo = 'The accommodation doesn\'t have any ratings yet';
        }
        });
      this.obtainUsersRatingForAccomm();
    }    
  }

  obtainUsersRatingForAccomm() {
    if(this.accommodation.id){
      this.ratingService.getUsersRating(this.accommodation.id).subscribe({
        next: (res: RatingDTO) => {
            this.hasRatedAccomm = true;
            this.existingAccommRating = res;
            this.userHostRatingInfo = 'You have rated this accommodation ' + this.existingAccommRating.grade;

        },
        error: (error: HttpErrorResponse) => {
          this.userAccommRatingInfo = "You haven\'t rated this accommodation yet";
        }
        });
    }
    this.obtainHostAveragerating();
  }
  obtainHostAveragerating() {
    if(this.accommodation.hostId){
      this.ratingService.getAverageRating(this.accommodation.hostId).subscribe({ 
        next: (res: RatedEntity) => {
          this.hostRatingInfo = 'Average host average rating is '+ res.averageRating;
        },
        error: (error: HttpErrorResponse) => {
          this.hostRatingInfo = 'The host doesn\'t have any ratings yet';
        }
        });
    }
    this.obtainUsersRatingForHost();
  }

  obtainUsersRatingForHost() {
    if(this.accommodation.hostId){
      this.ratingService.getUsersRating(this.accommodation.hostId).subscribe({
        next: (res: RatingDTO) => {
          this.hasRatedHost = true;
          this.existingHostRating = res;
          this.userHostRatingInfo = 'You have rated this host ' + this.existingHostRating.grade;
        },
        error: (error: HttpErrorResponse) => {
          this.userHostRatingInfo = "You haven\'t rated this host yet";
        }
       });
    }
  }
}

