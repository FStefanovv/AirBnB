import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { RatingDTO, RatingWithUsernameDTO } from 'src/app/model/ratings';
import { RatingServiceService } from 'src/app/services/rating-service.service';

@Component({
  selector: 'app-ratings',
  templateUrl: './ratings.component.html',
  styleUrls: ['./ratings.component.css']
})
export class RatingsComponent implements OnInit {

  @Input() entityId: string | undefined;
  @Input() ratingsFor: string = '';

  constructor(private ratingService: RatingServiceService) { }

  ratings: RatingWithUsernameDTO[] = [];


  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['entityId']) {
      this.getRatings();
    }
  }

  getRatings(){
    if(this.entityId){
      this.ratingService.getAllRatings(this.entityId).subscribe({
        next: (res: RatingWithUsernameDTO[]) => {
          console.log(res)
          this.ratings = res;
        },
        error: (err: HttpErrorResponse) => {
          console.log('error obtaining ratings for ', this.entityId)
        }
      });
    }
  }

}
