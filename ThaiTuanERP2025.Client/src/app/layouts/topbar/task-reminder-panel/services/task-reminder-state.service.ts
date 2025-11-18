// src/app/shared/services/alarm-state.service.ts
import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { TaskReminderApiService } from './task-reminder-api.service';
import { TaskReminderSignalRService } from './task-reminder.service';
import { TaskReminderDto } from '../models/task-reminder.model';

@Injectable({ providedIn: 'root' })
export class TaskReminderStateService {
      private readonly api = inject(TaskReminderApiService);
      private readonly realtime = inject(TaskReminderSignalRService);

      private readonly _reminders$ = new BehaviorSubject<TaskReminderDto[]>([]);
      readonly reminders$ = this._reminders$.asObservable();

      async init() {
            // load active reminders
            const list = await firstValueFrom(this.api.getMyActive());
            this._reminders$.next(list);

            // start SignalR
            const getToken = () => localStorage.getItem('token');
            await this.realtime.start(getToken);

            // incoming new reminders
            this.realtime.incoming$.subscribe(items => {
                  const current = this._reminders$.value;
                  const merged = [...items, ...current];
                  this._reminders$.next(merged);
            });

            // resolved reminders (approve / expired / dismissed)
            this.realtime.resolved$.subscribe(ids => {
                  for(let id of ids) console.log('id: ', id );
                  const left = this._reminders$.value.filter(a => !ids.includes(a.id));
                  this._reminders$.next(left);
            });
      }

      dismiss(id: string) {
            this.api.dismiss(id).subscribe(() => {
                  this._reminders$.next(this._reminders$.value.filter(a => a.id !== id));
            });
      }
}
