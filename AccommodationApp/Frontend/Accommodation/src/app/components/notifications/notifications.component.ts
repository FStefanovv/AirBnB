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


  ngOnInit(): void {
  }

}
