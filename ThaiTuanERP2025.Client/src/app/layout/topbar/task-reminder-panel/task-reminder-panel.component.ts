// src/app/layout/topbar/alarm-panel/alarm-panel.component.ts
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { Observable, interval, map, startWith } from 'rxjs';
import { TaskReminderDto } from './models/task-reminder.model';

@Component({
      selector: 'app-alarm-panel',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './task-reminder-panel.component.html',
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class TaskReminderPanelComponent {
      @Input() reminders$!: Observable<TaskReminderDto[]>;
      @Output() dismiss = new EventEmitter<string>();

      trackById(index: number, item: TaskReminderDto): string {
            return item.id;
      }

      leftTime(tr: TaskReminderDto): Observable<string> {
            return interval(1000).pipe(
                  startWith(0),
                  map(() => {
                        const sec = Math.max(0, Math.floor((new Date(tr.dueAt).getTime() - Date.now()) / 1000));
                        if (sec <= 0) return 'Hết hạn';
                        const h = Math.floor(sec / 3600);
                        const m = Math.floor((sec % 3600) / 60);
                        const s = sec % 60;
                        return `${h}h ${m}m ${s}s`;
                  })
            );
      }
}
