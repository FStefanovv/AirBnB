import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Flight } from 'src/app/model/flight';

@Component({
  selector: 'flight-card',
  templateUrl: './flight-card.component.html',
  styleUrls: ['./flight-card.component.css']
})
export class FlightCardComponent implements OnInit {

  constructor() { }

  @Input() flight?: Flight;
  @Output() cancelFlight = new EventEmitter<string>();

  ngOnInit(): void {
   
  }
  
  cancel (){
    if(this.flight){
      this.cancelFlight.emit(this.flight.id);
    }
  }
}
