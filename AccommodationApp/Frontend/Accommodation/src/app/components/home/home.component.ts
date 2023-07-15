import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccommodationDTO } from 'src/app/model/accommodation';
import { Address } from 'src/app/model/address';
import { CreateAccommodationDTO } from 'src/app/model/create-accommodation';
import { SearchDTO } from 'src/app/model/search';
import { AccommodationService } from 'src/app/services/accommodation.service';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  searchDTO: SearchDTO = new SearchDTO();
  allAccommodations: AccommodationDTO[] = [];
  filteredAccommodations : AccommodationDTO[] = [];
  searchedAccommodations : AccommodationDTO[] = [];
  accommodationsToShow : AccommodationDTO[] = [];
  lowestPrice : number = 0;
  highestPrice : number = 0;
  wifi : boolean = false;
  indoors : boolean = false;
  outdoors : boolean = false;
  netflix : boolean = false;
  parties : boolean = false;
  kitchen : boolean = false;
  smoking : boolean = false;
  pet : boolean = false;
  sauna : boolean = false;
  gym : boolean = false;
  isDistinguishedHost : boolean = false;
  isHostDistinguished: boolean = false;
  

  constructor(private accommodationService: AccommodationService, private userService: UserService, private authService : AuthService, private router: Router) {}

  ngOnInit(): void {
    this.showAll();
    this.IsHostDistinguished();
  }

  search() {
    console.log(this.searchDTO)
    this.accommodationService.search(this.searchDTO).subscribe({
      next: (res: any) => {
        this.searchedAccommodations = res;
        this.accommodationsToShow = this.searchedAccommodations
      }
    });
  }

  showAll() {
    this.accommodationService.getAll().subscribe({
      next : (res : any) => {
        this.allAccommodations = res
        this.accommodationsToShow = this.allAccommodations
      }
    })
  }

  filter(){
    if(this.wifi == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('WiFi'))
    }
    else{
      this.accommodationsToShow = this.searchedAccommodations
    }
    if(this.netflix == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Netflix'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.indoors == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Indoors pool'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.outdoors == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Outdoors pool'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.parties == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Parties allowed'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.kitchen == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Kitchen'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.smoking == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Smoking allowed'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.pet == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Pet friendly'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.sauna == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Sauna'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.gym == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.offers?.includes('Gym'))
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.isDistinguishedHost == true){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => item.isDistinguishedHost)
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.lowestPrice !== null && this.lowestPrice !== 0){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => this.lowestPrice <= <number>item.price)
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
    if(this.highestPrice !== null && this.highestPrice !== 0){
      this.accommodationsToShow = this.accommodationsToShow.filter(item => this.highestPrice >= <number>item.price)
    }
    else{
      this.accommodationsToShow = this.accommodationsToShow
    }
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

  sendRequest(id?:string){
      this.router.navigate(['/create-request',id]);
  }

  IsHostLoggedIn(): boolean {
    if (this.authService.getRole() === "HOST") {
      return true;
    }
    return false;
  } 

  IsHostDistinguished() {
    this.userService.getHost().subscribe({
      next: (res : any) => {
        if(res.isDistinguishedHost){
          console.log('dis je')
          this.isHostDistinguished = true;
        }
        else{
          console.log('nije dis')
          this.isHostDistinguished = false;
        }
      }
    })
  }

}





