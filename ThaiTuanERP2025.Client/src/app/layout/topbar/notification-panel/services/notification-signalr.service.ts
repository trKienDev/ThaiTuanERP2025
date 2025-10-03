import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { NotificationDto } from '../models/notification.model';

export interface NotificationPayload {
      userId: string;
      title: string;
      message: string;
      link: string;
      documentType: string;
      documentId: string;
      workflowInstanceId: string;
      workflowStepInstanceId: string;
}


@Injectable({ providedIn: 'root' })
export class NotificationSignalRService {
      private hubConnection?: signalR.HubConnection;

      // internal stream 
      private _incoming$ = new Subject<NotificationDto[]>();
      private _unreadCount$ = new BehaviorSubject<number>(0);

      // public stream
      readonly incoming$ = this._incoming$.asObservable();
      readonly unreadCount$: Observable<number> = this._unreadCount$.asObservable();

      /** Bắt đầu kết nối */
      start(getToken?: () => string | null) {
            if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
                  console.log('SignalR already connected');
                  return Promise.resolve();
            }
           
            this.hubConnection = new signalR.HubConnectionBuilder()
                  // .withUrl('https://localhost:7228/hubs/notifications', {  accessTokenFactory: () => getToken?.() ?? '' })
                  .withUrl(`${environment.baseUrl}${environment.hubs.notification}`, {  
                        accessTokenFactory: () => getToken?.() ?? '' 
                  })
                  .withAutomaticReconnect()
                  .build();

            // Lắng nghe sự kiện từ server
            this.hubConnection.on('ReceiveNotification', (payloads: NotificationDto[]) => {
                  console.log('ReceiveNotification:', payloads);

                  this._incoming$.next(payloads);
                  this._unreadCount$.next(this._unreadCount$.value + (payloads?.length ?? 0));
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

      markAllAsRead() {
            this._unreadCount$.next(0);
      }
}
