import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { TaskReminderStateService } from '../services/task-reminder-state.service';
import { TaskReminderDto } from '../models/task-reminder.model';

@Injectable({ providedIn: 'root' })
export class TaskReminderFacade {
      private readonly state = inject(TaskReminderStateService);
      readonly reminders$: Observable<TaskReminderDto[]> = this.state.reminders$;

      async init() {
            await this.state.init();
      }

      dismiss(id: string) { this.state.dismiss(id); }
}
