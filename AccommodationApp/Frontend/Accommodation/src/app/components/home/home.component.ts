import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccommodationService } from 'src/app/services/accommodation.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private accommService: AccommodationService) { }

  ngOnInit(): void {
  }



  downloadFiles() {
  this.accommService.GetPhotos('64345c35782e3689729e953b').subscribe({
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





}
