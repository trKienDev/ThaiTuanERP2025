import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class NotificationSignalRService {
      private hubConnection?: signalR.HubConnection;
      

      start(getToken?: () => string | null) {
            if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
                  console.log('SignalR already connected');
                  return Promise.resolve();
            }
           

            this.hubConnection = new signalR.HubConnectionBuilder()
                  // .withUrl('https://localhost:7228/hubs/notifications', {  accessTokenFactory: () => getToken?.() ?? '' })
                  .withUrl(`${environment.baseUrl}${environment.hubs.notification}`, {  accessTokenFactory: () => getToken?.() ?? '' })
                  .withAutomaticReconnect()
                  .build();

            this.hubConnection.on('ReceiveNotification', (payloads: any[]) => {
                  alert('ReceiveNotification: ' + JSON.stringify(payloads));
                  console.log('ReceiveNotification:', payloads);
            });

            return this.hubConnection.start()
                  .then(() => {
                        console.log('SignalR connected:', this.hubConnection?.state);
                        console.log(`hubs: ${environment.baseUrl}${environment.hubs.notification}`);
                  })
                  .catch(err => {
                        console.error('SignalR start error', err);
                  });
      }

      stop() {
            return this.hubConnection?.stop();
      }
}
