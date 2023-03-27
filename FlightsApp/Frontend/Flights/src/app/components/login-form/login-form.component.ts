import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { Credentials } from 'src/app/model/credentials';
import { TokenDTO } from 'src/app/model/tokenDto';
import { UserService } from 'src/app/services/users.service';
import { AuthService } from 'src/app/services/auth.service';


@Component({
  selector: 'login',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {

  constructor(private userService: UserService, private authService: AuthService, private router: Router) { }

  credentials: Credentials = new Credentials();
  showWrongCredentialsMessage: boolean = false;

  ngOnInit(): void {
  }
  
  logIn() :  void {
   if(this.credentials){
    this.userService.LogIn(this.credentials).subscribe({
      next: (response: TokenDTO) => {
        const token = response.token;
        if(token){
          this.authService.storeToken(token);
          this.router.navigate(['home']);
        } 
      },
      error : (err: HttpErrorResponse) => {
        this.showWrongCredentialsMessage = true;
      }
    });
   } 
  }
}


