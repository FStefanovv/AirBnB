import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ShowReservation } from 'src/app/model/show-reservation';
import { ReservationService } from 'src/app/services/reservation.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-reservations',
  templateUrl: './user-reservations.component.html',
  styleUrls: ['./user-reservations.component.css']
})
export class UserReservationsComponent implements OnInit {

  constructor(private reservationService: ReservationService, private userService: UserService,private router: Router) { }

  reservations?: ShowReservation[];

  ngOnInit(): void {
    this.reservationService.getUsersReservations().subscribe({
      next: (response: ShowReservation[]) => {
        this.reservations = response;
        console.log(this.reservations);
      },
      error : (err: HttpErrorResponse) => {
       console.log(err.error);
      }
    });
  }

  cancelReservation(reservation: ShowReservation){
    if(reservation.id)
      this.reservationService.cancel(reservation.id).subscribe({});
  }

  deleteAccAsGuest(){
    this.userService.deleteAccAsGuest().subscribe({
       next: (res: any) => {
      console.log('success');
      this.router.navigate(['login']);
    },
    error : (err: HttpErrorResponse) => {
     console.log(err);
    }
  });
  }

  getRecommendations(reservation: ShowReservation) {
    this.router.navigate(['flight-recommendations', reservation.id]);
  }

}
