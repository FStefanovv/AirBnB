import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateAccommodationDTO } from '../model/create-accommodation';
import { SearchDTO } from '../model/search';
import { Observable } from 'rxjs';
import { AccommodationDTO } from '../model/accommodation';
import { UpdateAccommodation } from '../model/update-accommodation';


@Injectable({
  providedIn: 'root'
})
export class AccommodationService {
  
  private accommUrl = 'http://localhost:5000/gateway/';
  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'multipart/form-data' })
  };

  httpOptions1 = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }
 
  
  Post(accommDto: CreateAccommodationDTO, images: File[]) {
    const form = new FormData;
    
    for(let image of images){
      form.append('file', image);
    }
    form.append('accomm-data', JSON.stringify(accommDto));

    return this.http.post(this.accommUrl+'create-accommodation', form);
  }

  search(searchDTO: SearchDTO) : Observable<AccommodationDTO[]> {
    return this.http.post<AccommodationDTO[]>(this.accommUrl + 'search-accommodation',searchDTO,this.httpOptions)
  }

  getAll() : Observable<AccommodationDTO[]>{
    return this.http.get<AccommodationDTO[]>(this.accommUrl + 'get-all',this.httpOptions)
  }
  
  GetPhotos(id: string) : Observable<Blob> {
    const headers = new HttpHeaders().set('Accept', 'application/octet-stream');
    return this.http.get(this.accommUrl + 'get-photos/'+id, { responseType: 'blob', headers });
  }

  updateAccommodation(updateAccommodation:UpdateAccommodation):Observable<UpdateAccommodation>
  {
    console.log(this.accommUrl+'update')
    console.log(this.updateAccommodation)

    return this.http.post(this.accommUrl+'update',updateAccommodation,this.httpOptions1)
  }
}
