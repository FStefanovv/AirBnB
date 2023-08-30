import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/model/user';
import { AuthService } from 'src/app/services/auth.service';
import { SignalRService } from 'src/app/services/signal-r.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private notificationService: SignalRService, private userService: UserService) { }

  loggedIn?: boolean;
  username?: string;
  userRole?: string;
  user?: User;

  ngOnInit(): void {
    this.loggedIn = this.authService.isLoggedIn();
    if(this.loggedIn){
      this.username = this.authService.getUsername();
      this.userRole = this.authService.getRole();
      this.user = this.userService.user;
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
