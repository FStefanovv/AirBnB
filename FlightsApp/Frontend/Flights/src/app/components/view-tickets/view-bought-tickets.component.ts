import { ViewTicketDto } from './../../model/viewTicketDto';
import { TicketsService } from './../../services/tickets.service';
import { AuthService } from 'src/app/services/auth.service';
import { Component, OnInit } from "@angular/core";
import { format, formatISO } from 'date-fns';

@Component({
  selector: 'view-bought-tickets',
  templateUrl: './view-bought-tickets.component.html',
  styleUrls: ['./view-bought-tickets.component.css']
})
export class ViewBoughtTickets implements OnInit{
  public tickets: any
  public proba: any
  public third_line_one: string="margin-left:10px"
  constructor(private authService:AuthService,private ticketService:TicketsService){}

  ngOnInit(): void{
    this.ticketService.viewTicketsForUser(this.authService.getId()).subscribe(res=>{
      this.tickets = res
      // console.log(format(this.tickets[0].departureTime,"MM/dd/yyyy hh:mm:ss AM"))
    })
  }
}