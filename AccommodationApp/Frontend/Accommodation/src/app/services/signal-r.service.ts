import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private notifications: Notification[] = [];
  
  private notificationsUrl = 'https://localhost:5000/gateway/notifications/';
  private gatewayUrl = 'https://localhost:5000/gateway/';

  private hubConnectionBuilder: signalR.HubConnection | undefined;

  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private authService: AuthService) { 
  }

  public init() {
    this.startConnection();
    this.addNotificationListener();
  }

  public getNotifications(){
    return this.notifications;
  }

  private startConnection() {
    this.hubConnectionBuilder?.stop();

    this.hubConnectionBuilder = new signalR.HubConnectionBuilder()
      .withUrl(this.notificationsUrl+this.authService.getId(), {
        withCredentials: false,
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();
      this.hubConnectionBuilder
        .start()
        .then(() => {
          console.log('Connection started.......!');
          if(this.hubConnectionBuilder)
            this.hubConnectionBuilder.invoke("CreateConnection")
              .then((connId)=>console.log(connId))
              .catch((err)=>console.log(err.toString()))
        })
        .catch(err => console.log('Error while connecting with server'));
  }

  public closeConnection(){
    this.hubConnectionBuilder?.stop();
    this.hubConnectionBuilder = undefined;
  }

  private addNotificationListener() {
    if(this.hubConnectionBuilder){
      this.hubConnectionBuilder.on('NewNotification', (data) => {
        this.notifications.unshift(data);
        console.log(data);
      });
    } 
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
}
