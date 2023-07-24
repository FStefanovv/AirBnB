import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccommodationDTO } from 'src/app/model/accommodation';
import { CreateRatingDTO, RatedEntity, RatingDTO, RatingInfoDTO } from 'src/app/model/ratings';
import { AccommodationService } from 'src/app/services/accommodation.service';
import { AuthService } from 'src/app/services/auth.service';
import { RatingServiceService } from 'src/app/services/rating-service.service';

@Component({
  selector: 'app-show-accommodation',
  templateUrl: './show-accommodation.component.html',
  styleUrls: ['./show-accommodation.component.css']
})
export class ShowAccommodationComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private accommodationService:AccommodationService, private ratingService: RatingServiceService, private authService: AuthService) { }

  accommodationId: string = '';
  accommodation: AccommodationDTO = new AccommodationDTO();

  createAccommodationRatingDto: CreateRatingDTO = new CreateRatingDTO();
  createHostRatingDto: CreateRatingDTO = new CreateRatingDTO();

 

  hasRatedAccomm: boolean = false;
  hasRatedHost: boolean = false;

  hostRatingInfo: string = '';
  accommRatingInfo: string = '';

  userHostRatingInfo: string = '';
  userAccommRatingInfo: string = '';

  pageRatingInfo: RatingInfoDTO[] = [];

  userRole: string = '';


  ngOnInit(): void {
    this.userRole = this.authService.getRole();
    const temp = this.activatedRoute.snapshot.paramMap.get("id");
    if(temp)
      this.accommodationId = temp;
    this.accommodationService.getAccommodation(this.accommodationId).subscribe(
      res =>
      {
        this.accommodation = res;
        this.obtainAllRatingInfo();
      }
    );
  }

  obtainAllRatingInfo() {
    if(this.accommodation.id && this.accommodation.hostId){
      this.ratingService.getPageRatingInfo(this.accommodation.id, this.accommodation.hostId).subscribe(
        res => {
          this.pageRatingInfo = res;
          this.displayRatingInfo();
        }
      );
    }
  }

  displayRatingInfo() {
    if(this.pageRatingInfo[0].grade==-1)
      this.accommRatingInfo = 'This accommodation has not been rated yet';
    else
      this.accommRatingInfo = 'Average accommodation rating is ' + this.pageRatingInfo[0].grade;

    if(this.pageRatingInfo[1].grade==-1)
      this.hostRatingInfo = 'This host has not been rated yet';
    else
      this.hostRatingInfo = 'Average host rating is ' + this.pageRatingInfo[1].grade;
    
    if(this.pageRatingInfo[2].grade==-1){
      this.userAccommRatingInfo = 'You haven\'t rated this accommodation yet';
      this.createAccommodationRatingDto.grade = 1;
    }
    else {
      this.userAccommRatingInfo = 'You have rated this accommodation with ';
      this.createAccommodationRatingDto.grade = this.pageRatingInfo[2].grade;
      this.hasRatedAccomm = true;
    }

    if(this.pageRatingInfo[3].grade==-1){
      this.userHostRatingInfo = 'You haven\'t rated this host yet';
      this.createHostRatingDto.grade = 1;
    }
    else {
      this.userHostRatingInfo = 'You have rated this host with ';
      this.createHostRatingDto.grade = this.pageRatingInfo[3].grade;
      this.hasRatedHost = true;
    }
  }

  rateAccomm(){
    this.createAccommodationRatingDto.ratedEntityId = this.accommodationId;
    this.createAccommodationRatingDto.ratedEntityType = 0;
    this.ratingService.rate(this.createAccommodationRatingDto).subscribe();
  }

  rateHost(){
    if(this.accommodation.hostId){
      this.createHostRatingDto.ratedEntityId = this.accommodation.hostId;
      this.createHostRatingDto.ratedEntityType = 1;
    }
    this.ratingService.rate(this.createHostRatingDto).subscribe();
  }

  deleteRating(ratingId: string){
    this.ratingService.deleteRating(ratingId).subscribe();
  }

  
}

