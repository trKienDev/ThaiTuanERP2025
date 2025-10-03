import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { TaskReminderStateService } from '../services/task-reminder-state.service';
import { TaskReminderDto } from '../models/task-reminder.model';

@Injectable({ providedIn: 'root' })
export class TaskReminderFacade {
  private state = inject(TaskReminderStateService);
  reminders$!: Observable<TaskReminderDto[]>;

  async init() {
    await this.state.init();
    this.reminders$ = this.state.reminders$;
  }

  dismiss(id: string) { this.state.dismiss(id); }
}
