import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { Notification } from '../models/notification';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private hubConnection!: signalR.HubConnection;

  constructor(
    private toastr: ToastrService
  ) {}

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}notify`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  };

  public addNotificationListner = () => {
    this.hubConnection.on('SendMessage', (notification: Notification) => {
      this.showNotification(notification)
    });
  };

  showNotification(notification: Notification) {
    this.toastr.success(
      notification.message
    );
  }
}
