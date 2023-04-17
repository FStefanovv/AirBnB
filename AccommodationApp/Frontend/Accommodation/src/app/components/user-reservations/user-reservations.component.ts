import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Reservation } from 'src/app/model/reservation';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-user-reservations',
  templateUrl: './user-reservations.component.html',
  styleUrls: ['./user-reservations.component.css']
})
export class UserReservationsComponent implements OnInit {

  constructor(private reservationService: ReservationService) { }

  reservations?: Reservation[];

  ngOnInit(): void {
    this.reservationService.getUsersReservations().subscribe({
      next: (response: Reservation[]) => {
        this.reservations = response;
        console.log(this.reservations);
      },
      error : (err: HttpErrorResponse) => {
       console.log(err.error);
      }
    });
  }

 

  cancelReservation(reservation: Reservation){
    if(reservation.id)
      this.reservationService.cancel(reservation.id).subscribe({});
  }

}
