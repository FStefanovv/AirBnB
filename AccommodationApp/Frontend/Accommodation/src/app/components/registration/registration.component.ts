import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Address } from 'src/app/model/address';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  user : User = new User()
  errorMsg : string = ''
  showMsg : boolean = false
  address : string = ''
  userAddress : Address = new Address()
  selected : boolean = true;
  greenMsg : boolean = false;
  constructor(private userService : UserService,private router : Router) { }

  ngOnInit(): void {
  }


  register(){
    this.userAddress.street = this.address.split(',')[0];
    this.userAddress.number = Number(this.address.split(',')[1]);
    this.userAddress.city = this.address.split(',')[2];
    this.userAddress.country = this.address.split(',')[3];
    this.user.address = this.userAddress;
    this.userService.Register(this.user).subscribe({
      next: (res: User) => {
        this.router.navigate(['login'])
      },
      error: (error : HttpErrorResponse) => {
        this.errorMsg = error.error;
        this.showMsg = true;
      }
    });
  }
}
