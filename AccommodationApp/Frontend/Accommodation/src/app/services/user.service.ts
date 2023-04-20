import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { LoginCredentials } from '../model/login-credentials';
import { Observable } from 'rxjs';
import { Token } from '../model/token';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl = 'http://localhost:5000/gateway/';
  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  LogIn(credentials: LoginCredentials) : Observable<Token> {
      return this.http.post<Token>(this.usersUrl+'login', credentials, this.httpOptions);
  }

  deleteAccAsGuest():Observable<any> {
    return this.http.delete(this.usersUrl+'deleteAsGuest1',this.httpOptions);
  }

  deleteAccAsHost():Observable<any> {
    return this.http.delete(this.usersUrl+'deleteAsHost',this.httpOptions);
  }
}
