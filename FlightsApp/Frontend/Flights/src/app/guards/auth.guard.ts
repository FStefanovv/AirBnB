import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
    constructor(private router:Router, private authService:AuthService){}
  
    canActivate(next: ActivatedRouteSnapshot) : Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if (this.authService.isLoggedIn()) {
        const userRole = this.authService.getRole();
        if (next.data['roles'] && next.data['roles'].indexOf(userRole) === -1) {
          this.router.navigate(['home']);
          return false;
        }
        return true;
      }
      
      this.router.navigate(['login']);
      return false;     
    }
}



