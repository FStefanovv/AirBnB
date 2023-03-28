import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  loggedIn?: boolean;
  username?: string;

  ngOnInit(): void {
    this.loggedIn = this.authService.isLoggedIn();
    if(this.loggedIn)
      this.username = this.authService.getUsername();
  }

  logIn(){
    this.router.navigate(['login']);
  }

  logOut(){
    this.authService.logOut();
    window.location.reload();
  }

}
