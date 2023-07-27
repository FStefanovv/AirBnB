import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ApiKeyDto, KeyValidUntilDto } from 'src/app/model/buyWithApiKey';
import { AuthService } from 'src/app/services/auth.service';
import { TicketsService } from 'src/app/services/tickets.service';

@Component({
  selector: 'app-generate-api-key',
  templateUrl: './generate-api-key.component.html',
  styleUrls: ['./generate-api-key.component.css']
})
export class GenerateApiKeyComponent implements OnInit {

  constructor(private ticketService: TicketsService, private authService: AuthService) { }

  dto: KeyValidUntilDto = new KeyValidUntilDto();

  keyGenerated: boolean = false;

  key: ApiKeyDto = new ApiKeyDto();

  permanentlyValid: boolean = false;

  validUntilMessage: string = '';

  ngOnInit(): void {
  }

  generateKey(){
    this.dto.userId = this.authService.getId();
    this.ticketService.generateKey(this.dto).subscribe({
      next: (res: ApiKeyDto) => {
        this.key = res;

        this.keyGenerated = true;
        if(this.permanentlyValid){
          this.validUntilMessage = 'Key valid permanently.';
        }
        else {
          this.validUntilMessage = 'Key valid until ' + this.key.validUntil+'.';
        }
      },
      error: (err: HttpErrorResponse) => {
        console.log('error generating api key');
      }
    });
  }

  onCheckboxChange(event: any) {
    if (event.target.checked) {
      this.permanentlyValid = true;
      this.dto.validUntil = new Date(2100, 1, 1);
    } else {
      this.permanentlyValid = false;
    }
  }

}
