import { ActivatedRoute } from '@angular/router';
import { AuthService } from './../../services/auth.service';
import { Component, OnInit } from "@angular/core";
import { CreateRequestDto } from 'src/app/model/createRequestDto';
import { ReservationService } from 'src/app/services/reservation.service';

@Component({
  selector:'create-request',
  templateUrl: './create-request.component.html',
  styleUrls: ['./create-request.component.css']
})
export class CreateRequestComponent implements OnInit{
  constructor(private authService: AuthService,private route: ActivatedRoute, private reservationService:ReservationService){}
  
  accommodationId?: string | null
  hostId?: string | null
  createRequestDto: CreateRequestDto = new CreateRequestDto()
  
  ngOnInit(): void {
    this.accommodationId = this.route.snapshot.queryParamMap.get('accommodationId')
    this.hostId = this.route.snapshot.queryParamMap.get('hostId')
  }


  createRequest(){
    if(typeof this.accommodationId === 'string'){
      this.createRequestDto.accomodationId = this.accommodationId
    }
    if(typeof this.hostId === 'string'){
      this.createRequestDto.hostId = this.hostId
    }
    this.createRequestDto.userId = this.authService.getId();
    this.reservationService.createRequest(this.createRequestDto).subscribe((res)=>{
      alert('sent')
      console.log(this.createRequestDto)
    })
  }
}