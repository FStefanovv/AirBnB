import { Component, OnInit } from '@angular/core';
import { LoginCredentials } from 'src/app/model/login-credentials';
import { FormsModule }   from '@angular/forms';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor() { }

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

    }
  }

}
