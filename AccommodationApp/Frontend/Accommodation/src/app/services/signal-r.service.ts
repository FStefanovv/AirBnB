import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { Notification } from '../model/notification';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public notifications: Notification[] = [];
  
  private notificationsUrl = 'https://localhost:5000/gateway/notifications/';
  private gatewayUrl = 'https://localhost:5000/gateway/';

  private hubConnectionBuilder: signalR.HubConnection | undefined;

  
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private authService: AuthService, private userService: UserService) { 
    window.addEventListener('load', this.init.bind(this));
  }

  public init() {
    this.startConnection();
    this.addNotificationListener();
    this.getUserNotifications();
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
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
      this.hubConnectionBuilder
        .start()
        .then(() => {
          console.log('Connection started.......!');
          if(this.hubConnectionBuilder)
            this.hubConnectionBuilder.invoke("CreateConnection")
              .then((connId)=>console.log('Connection ID is '+connId))
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
      this.hubConnectionBuilder.on('NewNotification', (data: Notification) => {
        this.handleNotification(data);
      });
    } 
  }

  handleNotification(data: Notification) {
    this.notifications.unshift(data);
    console.log(data);
    if(this.authService.getRole()=='HOST' && data.notificationContent.includes('host'))
      this.userService.setHost();
  }
  
  public getUserNotifications()  {
    this.fetchNotifications().subscribe((response) => {
      this.notifications = response; 
    });
  }

  fetchNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(this.gatewayUrl+'user-notifications', this.httpOptions);
  }
}


