import { Component, OnInit } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { Notification } from 'src/app/model/notification';
import { SignalRService } from 'src/app/services/signal-r.service';


@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {

  constructor(private notificationService: SignalRService) { }

  private hubConnectionBuilder!: signalR.HubConnection;

  ngOnInit(): void {
    if(!this.notificationService.connectionOpen){
      this.notificationService.startConnection();
      this.notificationService.addNotificationListener();
    }
    this.notificationService.getUserNotifications();
  }

}
