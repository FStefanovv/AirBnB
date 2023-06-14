import { Component, OnInit } from '@angular/core';
import { UpdateAccommodation } from 'src/app/model/update-accommodation';
import { ActivatedRoute } from '@angular/router';
import { AccommodationService } from 'src/app/services/accommodation.service';

@Component({
  selector: 'app-update-accommodation',
  templateUrl: './update-accommodation.component.html',
  styleUrls: ['./update-accommodation.component.css']
})
export class UpdateAccommodationComponent implements OnInit {

  updatedAccommodation:UpdateAccommodation=new UpdateAccommodation();
  priceError:boolean = false;

  constructor(private activatedRoute:ActivatedRoute,private accommodationService:AccommodationService) { }

  ngOnInit(): void {

    this.updatedAccommodation.id=this.activatedRoute.snapshot.paramMap.get("id") || "";
 
    this.updatedAccommodation.startSeason=this.activatedRoute.snapshot.paramMap.get("startSeason") || "";
 
    this.updatedAccommodation.endSeason=this.activatedRoute.snapshot.paramMap.get("endSeason") || "";
 
    this.updatedAccommodation.price=  Number.parseFloat(this.activatedRoute.snapshot.paramMap.get("price") || "")    
 



  }


  update(){ 
    if((this.updatedAccommodation.price || 0)<0){ this.priceError=true;}
   
   if(this.priceError==false){

    console.log(this.updatedAccommodation)
    this.accommodationService.updateAccommodation(this.updatedAccommodation).subscribe();
   }
   else(
    alert("Price is not valid")
   )

 
  }
}
