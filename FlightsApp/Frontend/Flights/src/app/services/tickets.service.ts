import { ViewTicketDto } from './../model/viewTicketDto';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BuyTicketDto } from '../model/buyTicketDto';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class TicketsService {
  private apiUrl ='https://localhost:5010/api/Tickets'

  token = this.authService.getToken();

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json', 'Authorization':`Bearer ${this.token}`})
  };

  constructor(private http: HttpClient, private authService:AuthService) { }

  buyTicket(dto: BuyTicketDto): Observable<BuyTicketDto>{
    return this.http.post<BuyTicketDto>(this.apiUrl,dto,this.httpOptions)
  }
  
  viewTicketsForUser(usernameId:string):Observable<ViewTicketDto[]>{
    return this.http.get<ViewTicketDto[]>(this.apiUrl+ "?userId=" +usernameId,this.httpOptions)
  }
}

