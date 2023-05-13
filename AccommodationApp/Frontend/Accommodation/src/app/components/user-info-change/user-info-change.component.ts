import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Address } from 'src/app/model/address';
import { User } from 'src/app/model/user';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-info-change',
  templateUrl: './user-info-change.component.html',
  styleUrls: ['./user-info-change.component.css']
})
export class UserInfoChangeComponent implements OnInit {

  constructor(private userService : UserService, private authService : AuthService) { }

  user : User = new User();
  addressSTR : string = ''
  password : string = ''
  passwordConfirmation : string = ''
  errorMessage: string = '';
  showError: boolean = false;
  newAddress : Address = new Address
  greenMsg : boolean = false;

  ngOnInit(): void {
    if(this.authService.getRole() == 'HOST'){
    this.userService.getHost().subscribe(res =>{
      this.user = res
      this.addressSTR = this.user.address?.street + ',' + this.user.address?.number + ',' + this.user.address?.city + ',' + this.user.address?.country
    })
    }
    else{
      this.userService.getRegular().subscribe(res =>{
        this.user = res
        this.addressSTR = this.user.address?.street + ',' + this.user.address?.number + ',' + this.user.address?.city + ',' + this.user.address?.country
      })
    }
  }


  updateUser(){
    this.user.password = this.password
    this.user.confirmPassword = this.passwordConfirmation
    this.newAddress.street = this.addressSTR.split(',')[0]
    this.newAddress.number = Number(this.addressSTR.split(',')[1])
    this.newAddress.city = this.addressSTR.split(',')[2]
    this.newAddress.country = this.addressSTR.split(',')[3]
    this.user.address = this.newAddress
    if(this.user.password !== this.user.confirmPassword){
      this.errorMessage = 'Password and confirm password must be the same';
      this.showError = true;
    }
    else if(this.user.password !== '' && this.user.password.length<6){
      this.errorMessage = 'Password needs to be at least 6 characters long';
      this.showError = true;
    }
    else{
    this.userService.updateUser(this.user).subscribe({
      });
      this.errorMessage = 'Account updated!';
      this.showError = true;
      this.greenMsg = true
    }
  }
}


