import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { NotificationDto } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationSignalRService {
      private hubConnection?: signalR.HubConnection;

      // internal stream 
      private readonly _incoming$ = new Subject<NotificationDto[]>();
      private readonly _unreadCount$ = new BehaviorSubject<number>(0);

      // public stream
      readonly incoming$ = this._incoming$.asObservable();
      readonly unreadCount$: Observable<number> = this._unreadCount$.asObservable();

      /** Bắt đầu kết nối */
      start(getToken?: () => string | null) {
            const token = getToken?.() ?? '';

            if (!token) {
                  console.error('❌ Không có access_token, SignalR không thể xác thực.');
                  return Promise.reject('No token');
            }

            this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl(`${environment.baseUrl}${environment.hubs.notification}`, {
                  accessTokenFactory: () => token
            })
            .configureLogging(signalR.LogLevel.Information)
            .withAutomaticReconnect()
            .build();

            this.hubConnection.on('ReceiveNotification', (payloads: NotificationDto[]) => {
                  this._incoming$.next(payloads);
                  this._unreadCount$.next(this._unreadCount$.value + (payloads?.length ?? 0));
            });

            return this.hubConnection.start()
                  .then(() => console.log('✅ SignalR connected:', this.hubConnection?.connectionId))
                  .catch(err => console.error('SignalR start error', err));
      }

      stop() {
            return this.hubConnection?.stop();
      }

      markAllAsRead() {
            this._unreadCount$.next(0);
      }
}
