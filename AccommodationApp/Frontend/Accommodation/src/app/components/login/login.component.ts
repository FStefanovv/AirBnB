import { Component, OnInit } from '@angular/core';
import { LoginCredentials } from 'src/app/model/login-credentials';
import { FormsModule }   from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { Token } from 'src/app/model/token';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { SignalRService } from 'src/app/services/signal-r.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private userService: UserService, private authService: AuthService, private router: Router, private notificationService: SignalRService) { }

  credentials: LoginCredentials = new LoginCredentials();
  errorMessage: string = '';
  showError: boolean = false;

  ngOnInit(): void {
  }


  login() {
    if(this.credentials.username=='' || this.credentials.password==''){
      this.errorMessage = 'Both username and password must be entered';
      this.showError = true;
    }
    else if(this.credentials.password.length<6){
      this.errorMessage = 'Password needs to be at least 6 characters long';
      this.showError = true;
    }
    else {
      this.userService.LogIn(this.credentials).subscribe({
        next: (response: Token) => {
          const token = response.token;
          if(token){
            this.authService.logIn(token);     
            this.notificationService.init();
            this.router.navigate(['my-reservations']);
          } 
        },
        error : (err: HttpErrorResponse) => {
          this.errorMessage = "Wrong username or password";
          this.showError = true;
        }
      });
    }
  }

}
