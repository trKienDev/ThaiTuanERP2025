import { Injectable, computed, inject } from '@angular/core';
import { TaskReminderStateService } from '../services/task-reminder-state.service';

@Injectable({ providedIn: 'root' })
export class TaskReminderFacade {
      private readonly state = inject(TaskReminderStateService);
      readonly reminders = computed(() => this.state.reminders());

      async init(): Promise<void> {
            await this.state.init();
      }

      dismiss(id: string): void {
            this.state.dismiss(id);
      }
}
