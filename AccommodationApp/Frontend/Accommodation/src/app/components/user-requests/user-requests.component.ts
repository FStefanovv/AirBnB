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

  hasRequests: boolean = false;

  ngOnInit(): void {
    this.getRequests();
  }

  requests?: ShowRequest[];

  getRequests(){
    this.reservationService.getRequests().subscribe({
      next: (res: ShowRequest[]) => {
        this.requests = res;
        if(this.requests.length!=0)
          this.hasRequests = true
      },
      error : (err: HttpErrorResponse) => {
      }
    });
  }
  cancelRequest(request: ShowRequest){
    if(request.requestId){
      this.reservationService.cancelRequest(request.requestId).subscribe(()=>{
        window.location.reload()
      });         
    }  
  }
}
