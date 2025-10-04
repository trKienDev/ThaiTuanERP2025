import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from "@angular/core";
import { interval, map, Observable, startWith } from "rxjs";
import { TaskReminderDto } from "../models/task-reminder.model";

@Component({
      selector: 'app-task-reminder-drawer',
      imports: [ CommonModule ],
      templateUrl: './task-reminder-drawer.component.html',
      styleUrls: ['./task-reminder-drawer.component.scss'],
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class TaskReminderDrawerComponent {
      @Input() reminders$!: Observable<TaskReminderDto[]>;
      @Output() dismiss = new EventEmitter<string>();
      @Output() closed = new EventEmitter<void>();

      trackById(index: number, item: TaskReminderDto) {
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

      onAnimationEnd(event: AnimationEvent) {
            if ((event.target as HTMLElement).classList.contains('closing')) {
                  this.closed.emit();
            }
      }
}
