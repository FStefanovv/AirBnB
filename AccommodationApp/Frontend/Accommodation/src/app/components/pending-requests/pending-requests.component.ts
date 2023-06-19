import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ReservationService } from 'src/app/services/reservation.service';
import { Request } from 'src/app/model/request';

@Component({
  selector: 'app-pending-requests',
  templateUrl: './pending-requests.component.html',
  styleUrls: ['./pending-requests.component.css']
})
export class PendingRequestsComponent implements OnInit {

  constructor(private reservationService: ReservationService) { }

  requests? : Request[];
  

  ngOnInit(): void {
    this.reservationService.getPendingRequests().subscribe({
      next: (response: any) => {
        this.requests = response;
        console.log(this.requests);
      },
      error : (err: HttpErrorResponse) => {
       console.log(err.error);
      }
    });

  }

}
