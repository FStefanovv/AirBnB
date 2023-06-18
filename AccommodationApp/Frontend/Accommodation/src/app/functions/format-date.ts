import { DatePipe } from "@angular/common";

export function formatDate(date: any): any {
    const datePipe = new DatePipe('en-US');

    const dateTransformed = datePipe.transform(date, 'dd-MMM-yyyy');
    if(dateTransformed)
      return dateTransformed;
  }