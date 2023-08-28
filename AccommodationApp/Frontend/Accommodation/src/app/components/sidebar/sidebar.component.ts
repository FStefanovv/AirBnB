import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { SignalRService } from 'src/app/services/signal-r.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private notificationService: SignalRService) { }

  loggedIn?: boolean;
  username?: string;
  userRole?: string;

  ngOnInit(): void {
    this.loggedIn = this.authService.isLoggedIn();
    if(this.loggedIn){
      this.username = this.authService.getUsername();
      this.userRole = this.authService.getRole();
    }
  }

  logIn(){
    this.router.navigate(['login']);
  }

  logOut(){
    this.authService.logOut();
    this.notificationService.closeConnection();
    if(this.router.url=='/home')
      window.location.reload();
    else this.router.navigate(['home']);
  }

  goToRegistration(){
    this.router.navigate(['register-user']);
  }

  changeRoute(url:string){
    this.router.navigate([url])
  }


}
