import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ShowReservation } from 'src/app/model/show-reservation';
import { ReservationService } from 'src/app/services/reservation.service';
import { UserService } from 'src/app/services/user.service';
import { ReservationStatus } from 'src/app/model/show-reservation';
import { ShowRequest } from 'src/app/model/show-requests';

@Component({
  selector: 'app-user-reservations',
  templateUrl: './user-reservations.component.html',
  styleUrls: ['./user-reservations.component.css']
})
export class UserReservationsComponent implements OnInit {

  constructor(private reservationService: ReservationService,private router: Router) { }

  reservations?: ShowReservation[];
  requests?: ShowRequest[];

  ngOnInit(): void {
    this.getReservations();
  }

  getReservations(){
    this.reservationService.getUsersReservations().subscribe({
      next: (response: ShowReservation[]) => {
        this.reservations = response;
        
      },
      error : (err: HttpErrorResponse) => {
       console.log(err.error);
      }
    });
  }

  cancelReservation(reservation: ShowReservation){
    console.log(reservation)
    if(reservation.id){
      if(reservation.hostId){
        this.reservationService.cancelReservation(reservation.id,reservation.hostId).subscribe({});
      }
    }
  }

  getRecommendations(reservation: ShowReservation) {
    this.router.navigate(['flight-recommendations', reservation.id]);
  }

  canBeCancelled(reservationStart: Date | undefined) : boolean {
    return true;
  }

}
