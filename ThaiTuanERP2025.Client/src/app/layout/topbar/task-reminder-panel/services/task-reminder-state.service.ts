// src/app/shared/services/alarm-state.service.ts
import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, firstValueFrom, interval, map } from 'rxjs';
import { TaskReminderApiService } from './task-reminder-api.service';
import { TaskReminderSignalRService } from './task-reminder.service';
import { TaskReminderDto } from '../models/task-reminder.model';

@Injectable({ providedIn: 'root' })
export class TaskReminderStateService {
      private api = inject(TaskReminderApiService);
      private realtime = inject(TaskReminderSignalRService);

      private _reminders$ = new BehaviorSubject<TaskReminderDto[]>([]);
      readonly reminders$ = this._reminders$.asObservable();

      async init() {
            // load active reminders
            const list = await firstValueFrom(this.api.getMyActive());
            this._reminders$.next(list);

            // start SignalR
            const getToken = () => localStorage.getItem('access_token');
            await this.realtime.start(getToken);

            // incoming new reminders
            this.realtime.incoming$.subscribe(items => {
                  const current = this._reminders$.value;
                  this._reminders$.next([...items, ...current]);
            });

            // resolved reminders (approve / expired / dismissed)
            this.realtime.resolved$.subscribe(ids => {
                  const left = this._reminders$.value.filter(a => !ids.includes(a.id));
                  this._reminders$.next(left);
            });

            // optional: client-side auto-drop when due passes (fallback)
            interval(1000 * 30).subscribe(() => {
                  const now = Date.now();
                  const left = this._reminders$.value.filter(a => new Date(a.dueAt).getTime() > now);
                  if (left.length !== this._reminders$.value.length) 
                        this._reminders$.next(left);
            });
      }

      dismiss(id: string) {
            this.api.dismiss(id).subscribe(() => {
                  this._reminders$.next(this._reminders$.value.filter(a => a.id !== id));
            });
      }
}
