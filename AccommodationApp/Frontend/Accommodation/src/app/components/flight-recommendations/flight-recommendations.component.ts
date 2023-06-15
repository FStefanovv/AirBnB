import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShowReservation } from 'src/app/model/show-reservation';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-flight-recommendations',
  templateUrl: './flight-recommendations.component.html',
  styleUrls: ['./flight-recommendations.component.css']
})
export class FlightRecommendationsComponent implements OnInit {

  reservation?: ShowReservation;

  constructor(private activatedRoute : ActivatedRoute, private reservationService : ReservationService) { }

  ngOnInit(): void {
    const reservationId = this.activatedRoute.snapshot.paramMap.get("id");
    if(reservationId){
      this.reservationService.getReservation(reservationId).subscribe(
        res => {
          this.reservation = res;
          console.log(this.reservation);
      });
        
      
    }
  }

}
