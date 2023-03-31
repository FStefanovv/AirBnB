import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RegistrationData } from 'src/app/model/registrationData';
import { SuccessfulRegistraionDTO } from 'src/app/model/successfulRegistrationDto';
import { UserService } from 'src/app/services/users.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(private userService: UserService, private router: Router) { }

  registrationData: RegistrationData = new RegistrationData();
  registrationFailed: boolean = false;
  registrationError: string = '';
  not_registered: boolean = true;
  successfulRegistrationDTO?: SuccessfulRegistraionDTO;

  ngOnInit(): void {
  }
//{{successfulRegistrationDTO?.username}}
  register() {
    if(this.registrationData){
        if(this.fieldsEmpty()) {
          this.registrationFailed = true;
          this.registrationError = 'Please fill out all the fields';
          return;
        }
        const passwordsError = this.validatePasswords();
        if(passwordsError) {
          this.registrationFailed = true;
          this.registrationError = passwordsError;
          return;
        }

        this.userService.Register(this.registrationData).subscribe({
          next: (res: SuccessfulRegistraionDTO) => {
            this.successfulRegistrationDTO = res;
            this.not_registered = false;

          },
          error: (error: HttpErrorResponse) => {
            this.registrationFailed = true;
            this.registrationError = error.error;
          }
         })
    }
  }

  fieldsEmpty() : boolean {
    if(this.registrationData.firstName=='' || this.registrationData.lastName=='' || 
      this.registrationData.eMail=='' || this.registrationData.username=='' || 
      this.registrationData.password=='' || this.registrationData.confirmPassword=='')
      return true;
    return false;
  }

  validatePasswords() : any {

    if(this.registrationData.password!=this.registrationData.confirmPassword)
      return "Passwords don't match";
    else if(this.registrationData.password && this.registrationData.password.length <= 8)
      return "Password too short, needs to contain at least 9 characters";
    return undefined;
  }

  goToLogin() {
    this.router.navigate(['login']);
  }
  

}
