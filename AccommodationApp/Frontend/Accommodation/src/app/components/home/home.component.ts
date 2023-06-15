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
  accommodations: AccommodationDTO[] = [];

  constructor(private accommodationService: AccommodationService, private router: Router) {}

  ngOnInit(): void {
    this.showAll();
  }

  search() {
    this.searchDTO.location = this.address; 
    console.log(this.searchDTO)
    this.accommodationService.search(this.searchDTO).subscribe({
      next : (res : any) => {
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
}

