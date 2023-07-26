import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  loggedIn?: boolean;
  username?: string;
  userRole?: string;

  ngOnInit(): void {
    this.loggedIn = this.authService.isLoggedIn();
    console.log('jel ulogovan', this.loggedIn)
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
    window.location.reload();
  }

  goToRegistration(){
    this.router.navigate(['register-user']);
  }

  changeRoute(url:string){
    this.router.navigate([url])
  }


}
