import { ViewTicketDto } from './../model/viewTicketDto';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BuyTicketDto } from '../model/buyTicketDto';
import { ApiKeyDto, BuyTicketApiKeyDto, KeyValidUntilDto } from '../model/buyWithApiKey';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class TicketsService {
  private apiUrl ='https://localhost:5010/api/Tickets'

  getHttpHeadersRegular() : HttpHeaders {
    
    return  new HttpHeaders({'Content-Type': 'application/json'})
  }

  getHttpHeadersBearer() : HttpHeaders {
    const token = this.authService.getToken();
    
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization':`Bearer ${token}`});
  }

  constructor(private http: HttpClient, private authService: AuthService) { }

  buyTicket(dto: BuyTicketDto): Observable<BuyTicketDto>{
    return this.http.post<BuyTicketDto>(this.apiUrl,dto,{headers: this.getHttpHeadersBearer()})
  }
  
  viewTicketsForUser(usernameId:string):Observable<ViewTicketDto[]>{
    return this.http.get<ViewTicketDto[]>(this.apiUrl+ "?userId=" +usernameId,{headers: this.getHttpHeadersBearer()})
  }

 purchaseTicketApikey(dto: BuyTicketApiKeyDto, apiKey: string): Observable<BuyTicketApiKeyDto>{
    const apiKeyHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    'Api-Key': apiKey
    });

    const httpOptionsApiKey = {
      headers: apiKeyHeaders
    };
    return this.http.post<BuyTicketApiKeyDto>(this.apiUrl+'/buy-with-api-key',dto,httpOptionsApiKey)
  }

  generateKey(validUntil: KeyValidUntilDto) : Observable<ApiKeyDto>{
    
    return this.http.post<ApiKeyDto>(this.apiUrl+'/generate-api-key',validUntil,{headers: this.getHttpHeadersBearer()});
  }
}

