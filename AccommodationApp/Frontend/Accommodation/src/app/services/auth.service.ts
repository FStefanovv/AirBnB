import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { SignalRService } from './signal-r.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private jwtHelper: JwtHelperService, private notificationService: SignalRService) { }

  isLoggedIn() {
    const token = this.getToken();
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }

  public decodeToken() {
    const token = this.getToken();
    if(token)
      return this.jwtHelper.decodeToken(token);
  }

  getRole(){
    if(this.isLoggedIn()){
      const decodedToken = this.decodeToken();
      return decodedToken['Role'];
    }
    return null;
  }

  getUsername(){
    const decodedToken = this.decodeToken();
    return decodedToken['Username'];
  }

  getId(){
    const decodedToken = this.decodeToken()
    return decodedToken['UserId'];
  }

  getToken(){
    return localStorage.getItem("jwt");
  }

  storeToken(token: string){
    localStorage.setItem("jwt", token); 
  }

  logOut(){
    localStorage.removeItem("jwt");
    this.notificationService.closeConnection();
  }
}
