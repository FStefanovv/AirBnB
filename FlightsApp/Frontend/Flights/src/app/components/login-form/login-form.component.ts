import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Credentials } from 'src/app/model/credentials';
import { TokenDTO } from 'src/app/model/tokenDto';
import { UserService } from 'src/app/services/users.service';


@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {

  constructor(private userService: UserService) { }

  credentials: Credentials = new Credentials();
  showWrongCredentialsMessage: boolean = false;

  ngOnInit(): void {
  }

  logIn() :  void {
   if(this.credentials){
    this.userService.LogIn(this.credentials).subscribe({
      next: (response: TokenDTO) => {
        const token = response.token;
        if(token)
          localStorage.setItem("jwt", token); 
      },
      error : (err: HttpErrorResponse) => {
        this.showWrongCredentialsMessage = true;
      }
    });
   } 
  }
}


