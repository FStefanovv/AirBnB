import { AuthService } from './../../services/auth.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Flight } from 'src/app/model/flight';

@Component({
  selector: 'flight-card',
  templateUrl: './flight-card.component.html',
  styleUrls: ['./flight-card.component.css']
})
export class FlightCardComponent implements OnInit {

  constructor(private authService: AuthService) { }

  @Input() flight?: Flight;
  @Output() cancelFlight = new EventEmitter<string>();

  loggedIn?: Boolean
  userRole?: string

  ngOnInit(): void {
    this.loggedIn = this.authService.isLoggedIn()
    if(this.loggedIn){
      this.userRole = this.authService.getRole()
    }
  }
  
  cancel (){
    if(this.flight){
      this.cancelFlight.emit(this.flight.id);
    }
  }
}
