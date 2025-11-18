// src/app/shared/services/alarm-state.service.ts
import { DestroyRef, Injectable, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { firstValueFrom } from 'rxjs';
import { TaskReminderApiService } from './task-reminder-api.service';
import { TaskReminderSignalRService } from './task-reminder.service';
import { TaskReminderDto } from '../models/task-reminder.model';

@Injectable({ providedIn: 'root' })
export class TaskReminderStateService {
      private readonly api = inject(TaskReminderApiService);
      private readonly realtime = inject(TaskReminderSignalRService);
      private readonly destroyRef = inject(DestroyRef);

      private initialized = false;

      // ===== Signals =====
      private readonly _reminders = signal<TaskReminderDto[]>([]);
      readonly reminders = computed(() => this._reminders());

      async init(): Promise<void> {
            if (this.initialized) return;
            this.initialized = true;

            // 1. load active reminders
            const list = await firstValueFrom(this.api.getMyActive());
            this._reminders.set(list);

            // 2. start SignalR
            const getToken = () => localStorage.getItem('token');
            await this.realtime.start(getToken);

            // 3. incoming new reminders
            this.realtime.incoming$.pipe(takeUntilDestroyed(this.destroyRef))
                  .subscribe(items => {
                        this._reminders.update(current => [...items, ...current]);
                  });

            // 4. resolved reminders (approve / expired / dismissed)
            this.realtime.resolved$.pipe(takeUntilDestroyed(this.destroyRef))
                  .subscribe(ids => {
                        if (!Array.isArray(ids)) {
                              console.error('ResolveReminder payload không phải array:', ids);
                              return;
                        }

                        this._reminders.update(current =>
                              current.filter(r => !ids.includes(r.id))
                        );
                  });
      }

      dismiss(id: string): void {
            this.api.dismiss(id)
                  .pipe(takeUntilDestroyed(this.destroyRef))
                  .subscribe(() => {
                        this._reminders.update(current => current.filter(r => r.id !== id));
                  });
      }
}

