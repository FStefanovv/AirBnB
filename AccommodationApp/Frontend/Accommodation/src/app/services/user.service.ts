import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { LoginCredentials } from '../model/login-credentials';
import { Observable } from 'rxjs';
import { Token } from '../model/token';
import { User } from '../model/user';


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

  getHost() : Observable<User> {
    return this.http.get<User>(this.usersUrl+'get-host', this.httpOptions);
  }

  updateUser(user : User) : Observable<User>{
    return this.http.post<User>(this.usersUrl + 'update-user',user,this.httpOptions);
  }

  Register(user : User) : Observable<User>{
    return this.http.post<User>(this.usersUrl + 'register-user',user,this.httpOptions);
  }

  getRegular() : Observable<User>{
    return this.http.get<User>(this.usersUrl+'get-regular',this.httpOptions);
  }

  deleteAccAsGuest():Observable<any> {
    return this.http.delete(this.usersUrl+'deleteAsGuest1',this.httpOptions);
  }

  deleteAccAsHost():Observable<any> {
    return this.http.delete(this.usersUrl+'deleteAsHost',this.httpOptions);
  }
}
