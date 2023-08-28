import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public notifications: Notification[] = [];

  public connectionOpen: boolean = false;
  
  private notificationsUrl = 'https://localhost:5000/gateway/notifications';
  private gatewayUrl = 'https://localhost:5000/gateway/';

  private hubConnectionBuilder!: signalR.HubConnection;

  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  public startConnection() {
    this.hubConnectionBuilder = new signalR.HubConnectionBuilder()
      .withUrl(this.notificationsUrl, {
        withCredentials: false,
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();
      this.hubConnectionBuilder
        .start()
        .then(() => console.log('Connection started.......!'))
        .catch(err => console.log('Error while connecting with server'));

    this.connectionOpen = true;
  }

  public addNotificationListener() {
    this.hubConnectionBuilder.on('NewNotification', (data) => {
      this.notifications.unshift(data);
      console.log(data);
    }); 
  }
  
  public getUserNotifications()  {
    this.fetchNotifications().subscribe((response) => {
      this.notifications = response; 
      console.log(this.notifications);
    });
  }

  fetchNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(this.gatewayUrl+'user-notifications', this.httpOptions);
  }

  closeConnection() {
    this.connectionOpen = false;
    this.hubConnectionBuilder.stop()
  }
}
