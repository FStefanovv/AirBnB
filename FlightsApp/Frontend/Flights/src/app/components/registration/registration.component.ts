import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RegistrationData } from 'src/app/model/registrationData';
import { SuccessfulRegistraionDTO } from 'src/app/model/successfulRegistrationDto';
import { UserService } from 'src/app/services/users.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(private userService: UserService) { }

  registrationData: RegistrationData = new RegistrationData();
  registrationFailed: boolean = false;
  registrationError: string = '';

  ngOnInit(): void {
  }

  register() {
    if(this.registrationData){
        this.userService.Register(this.registrationData).subscribe({
          next: (res: SuccessfulRegistraionDTO) => {
            alert(res.username);
          },
          error: (error: HttpErrorResponse) => {
            this.registrationFailed = true;
            this.registrationError = error.message;
          }
         })
    }
  }

}
