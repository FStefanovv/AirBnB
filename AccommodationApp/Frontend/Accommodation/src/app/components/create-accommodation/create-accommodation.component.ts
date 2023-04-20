import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { CreateAccommodationDTO } from 'src/app/model/create-accommodation';
import { AccommodationService } from 'src/app/services/accommodation.service';
import { UserService } from 'src/app/services/user.service';


@Component({
  selector: 'app-create-accommodation',
  templateUrl: './create-accommodation.component.html',
  styleUrls: ['./create-accommodation.component.css']
})
export class CreateAccommodationComponent implements OnInit {

  constructor(private accommodationService: AccommodationService, private userService: UserService,private router: Router) { }

  dropdownList = [
      { item_id: 1, item_text: 'WiFi' },
      { item_id: 2, item_text: 'Indoors pool' },
      { item_id: 3, item_text: 'Outdoors pool' },
      { item_id: 4, item_text: 'Netflix' },
      { item_id: 5, item_text: 'Parties allowed' },
      { item_id: 6, item_text: 'Kitchen' },
      { item_id: 7, item_text: 'Smoking allowed' },
      { item_id: 8, item_text: 'Pet friendly' },
      { item_id: 9, item_text: 'Sauna' },
      { item_id: 6, item_text: 'Gym' }
  ];

  dropdownSettings : IDropdownSettings = {
    singleSelection: false,
    idField: 'item_id',
    textField: 'item_text',
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 5,
    allowSearchFilter: true
  };

  selectedItems: any[] = [];

  accommDto: CreateAccommodationDTO = new CreateAccommodationDTO();
  images: File[] = [];

  guestNumError = false;

  ngOnInit(): void {    
  }

  postAccommodation() {
    if(this.accommDto.maxGuests && this.accommDto.minGuests && this.accommDto.maxGuests < this.accommDto.minGuests){
      this.guestNumError = true;
    }
    for(let item of this.selectedItems){
      this.accommDto.offers?.push(item.item_text);
    }
    this.accommodationService.Post(this.accommDto, this.images).subscribe({
      next: (response: any) => {
        console.log('success');
      },
      error : (err: HttpErrorResponse) => {
       console.log(err);
      }
    });
  }

  selectImages(event: any) {
    this.images = event.target.files;
  }

  deleteAccAsHost(){
    this.userService.deleteAccAsHost().subscribe({
      next: (res: any) => {
     console.log('success');
     this.router.navigate(['login'])
   },
   error : (err: HttpErrorResponse) => {
    console.log(err);
   }
 });
    
  }

}
