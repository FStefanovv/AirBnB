import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccommodationRecommendation } from 'src/app/model/accomm-recomm';
import { RatingServiceService } from 'src/app/services/rating-service.service';

@Component({
  selector: 'app-accommodation-recommendations',
  templateUrl: './accommodation-recommendations.component.html',
  styleUrls: ['./accommodation-recommendations.component.css']
})
export class AccommodationRecommendationsComponent implements OnInit {

  constructor(private ratingService: RatingServiceService, private router: Router) { }
  recommendations: AccommodationRecommendation[] = [];
  hasRecommendations?: boolean;

  ngOnInit(): void {
    this.ratingService.getAccommodationRecomemendations().subscribe((res)=>{
      this.recommendations = res;
      if(this.recommendations.length!=0)
        this.hasRecommendations = true;
      else
        this.hasRecommendations = false;
    })
  }

  goToAccommodation(id:string) {
    this.router.navigate(['/show-accommodation',id]); 
  }

}
