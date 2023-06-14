import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccommodationDTO } from 'src/app/model/accommodation';
import { Address } from 'src/app/model/address';
import { CreateAccommodationDTO } from 'src/app/model/create-accommodation';
import { SearchDTO } from 'src/app/model/search';
import { AccommodationService } from 'src/app/services/accommodation.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  searchDTO: SearchDTO = new SearchDTO();
  address: string = '';
  newAddress: Address = new Address();
  accommodations: AccommodationDTO[] = [];
  addressParts : string[] = []

  constructor(private accommodationService: AccommodationService, private router: Router) {}

  ngOnInit(): void {
    this.showAll();
  }

  search() {
    this.addressParts = this.address.split(',');
    this.newAddress.street = this.addressParts[0];
    this.newAddress.number = Number(this.addressParts[1]);
    this.newAddress.city = this.addressParts[2];
    this.newAddress.country = this.addressParts[3];
    this.searchDTO.location = this.newAddress;
    console.log(this.searchDTO)
    this.accommodationService.search(this.searchDTO).subscribe({
      next: (res: any) => {
        this.accommodations = res;
      }
    });
  }

  showAll() {
    this.accommodationService.getAll().subscribe({
      next : (res : any) => {
        this.accommodations = res
      }
    })
  }

  downloadFiles() {
  this.accommodationService.GetPhotos('64345c35782e3689729e953b').subscribe({
    next: (response: any) => {
      /*
      console.log(response)
      const blob = new Blob([response], { type: 'application/octet-stream' });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = 'files.zip';
      link.click();
      window.URL.revokeObjectURL(url);*/
    },
    error : (err: HttpErrorResponse) => {
     console.log('jebigica')
    }
   });
  }

  update(id?:string,startSeason?:string,endSeason?:string,price?:number){

    this.router.navigate(
      ['/update-accommodation',id,startSeason,endSeason,price] ); 

  }

    
}



