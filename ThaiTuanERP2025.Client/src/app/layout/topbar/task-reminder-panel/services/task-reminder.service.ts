import { Injectable } from "@angular/core";
import * as signalR from '@microsoft/signalr';
import { TaskReminderDto } from "../models/task-reminder.model";
import { Observable, Subject } from "rxjs";
import { environment } from "../../../../../environments/environment";

@Injectable({ providedIn: 'root' })
export class TaskReminderSignalRService {
      private hub?: signalR.HubConnection;

      private _incoming$ = new Subject<TaskReminderDto[]>(); // ReceiveAlarm
      private _resolved$ = new Subject<string[]>();           // ResolveAlarm (list id)
      readonly incoming$: Observable<TaskReminderDto[]> = this._incoming$.asObservable();
      readonly resolved$: Observable<string[]> = this._resolved$.asObservable();

      async start(getToken?: () => string | null) {
            if (this.hub?.state === signalR.HubConnectionState.Connected) return;
            this.hub = new signalR.HubConnectionBuilder()
                  .withUrl(`${environment.baseUrl}${environment.hubs.notification}`, {
                        accessTokenFactory: () => getToken?.() ?? ''
                  })
                  .withAutomaticReconnect()
                  .build();

            this.hub.on('ReceiveReminder', (payloads: TaskReminderDto[]) => this._incoming$.next(payloads));
            this.hub.on('ResolveReminder', (alarmIds: string[]) => this._resolved$.next(alarmIds));

            await this.hub.start();
      }

      stop() { return this.hub?.stop(); }
}