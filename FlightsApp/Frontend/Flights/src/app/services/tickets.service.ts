import { ViewTicketDto } from './../model/viewTicketDto';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BuyTicketDto } from '../model/buyTicketDto';
import { BuyTicketApiKeyDto } from '../model/buyWithApiKey';

@Injectable({ providedIn: 'root' })
export class TicketsService {
  private apiUrl ='https://localhost:5010/api/Tickets'

  httpOptions ={
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  }

  constructor(private http: HttpClient) { }

  buyTicket(dto: BuyTicketDto): Observable<BuyTicketDto>{
    return this.http.post<BuyTicketDto>(this.apiUrl,dto,this.httpOptions)
  }
  
  viewTicketsForUser(usernameId:string):Observable<ViewTicketDto[]>{
    return this.http.get<ViewTicketDto[]>(this.apiUrl+ "?userId=" +usernameId,this.httpOptions)
  }

 purchaseTicketApikey(dto: BuyTicketApiKeyDto, apiKey: string): Observable<BuyTicketApiKeyDto>{
    const apiKeyHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    'Api-Key': apiKey
    });

    const httpOptionsApiKey = {
      headers: apiKeyHeaders
    };
    return this.http.post<BuyTicketApiKeyDto>(this.apiUrl,dto,httpOptionsApiKey)
  }
}

