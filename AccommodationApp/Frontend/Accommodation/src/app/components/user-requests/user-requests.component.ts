import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ShowRequest } from 'src/app/model/show-requests';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector: 'app-user-requests',
  templateUrl: './user-requests.component.html',
  styleUrls: ['./user-requests.component.css']
})
export class UserRequestsComponent implements OnInit {

  constructor(private reservationService: ReservationService) { }

  ngOnInit(): void {
    this.getRequests();
  }

  requests?: ShowRequest[];

  getRequests(){
    this.reservationService.getRequests().subscribe({
      next: (res: ShowRequest[]) => {
        this.requests = res;
        console.log(this.requests)
      },
      error : (err: HttpErrorResponse) => {
       console.log(err.error);
      }
    });
  }
  cancelRequest(request: ShowRequest){
    console.log(request.requestId)
    if(request.requestId){
      console.log('uso')
      this.reservationService.cancelRequest(request.requestId).subscribe({});    
    }  
  }
}
