import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrls: ['./delete-account.component.css']
})
export class DeleteAccountComponent implements OnInit {

  constructor(private userService: UserService, private authService: AuthService) { }

  ngOnInit(): void {
  }

  deleteAccount() : void {
    const role = this.authService.getRole();
    if(role=="HOST"){
      this.userService.deleteAccAsHost().subscribe();
    }
    else {
      this.userService.deleteAccAsGuest().subscribe();
    }
  }

}
