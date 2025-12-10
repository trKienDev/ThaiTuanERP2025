import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, computed, DestroyRef, EventEmitter, inject, Input, OnInit, Output, signal, Signal } from "@angular/core";
import { TaskReminderDto } from "../models/task-reminder.model";
import { Router } from "@angular/router";

@Component({
      selector: 'app-task-reminder-drawer',
      imports: [ CommonModule ],
      templateUrl: './task-reminder-drawer.component.html',
      styleUrls: ['./task-reminder-drawer.component.scss'],
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class TaskReminderDrawerComponent {
      @Input({ required: true }) reminders!: Signal<TaskReminderDto[]>;
      @Output() dismiss = new EventEmitter<string>();
      @Output() closed = new EventEmitter<void>();

      private readonly router = inject(Router);
      private readonly destroyRef = inject(DestroyRef);

      readonly now = signal(Date.now());

      constructor() {
            const id = setInterval(() => {
                  this.now.set(Date.now());
            }, 1000);

            this.destroyRef.onDestroy(() => clearInterval(id));
      }

      readonly sortedReminders = computed(() => {
            const list = this.reminders();
            const now = this.now();

            return [...list].sort((a, b) => {
                  const aDue = new Date(a.dueAt).getTime();
                  const bDue = new Date(b.dueAt).getTime();
                  const aExpired = aDue <= now;
                  const bExpired = bDue <= now;

                  if (aExpired !== bExpired) return aExpired ? 1 : -1; // còn hạn trước
                  return aDue - bDue; // cùng nhóm: dueAt sớm trước
            });
      });

      leftTime(tr: TaskReminderDto): string {
            const sec = Math.max(
                  0,
                  Math.floor((new Date(tr.dueAt).getTime() - this.now()) / 1000)
            );
            if (sec <= 0) return 'Hết hạn';
            const h = Math.floor(sec / 3600);
            const m = Math.floor((sec % 3600) / 60);
            const s = sec % 60;
            return `${h}h ${m}m ${s}s`;
      }

      isExpired(item: TaskReminderDto): boolean {
            return new Date(item.dueAt).getTime() <= this.now();
      }

      onClickItem(reminder: TaskReminderDto) {
            if (reminder.linkUrl) window.open(reminder.linkUrl, '_blank');
      }

      trackById(index: number, item: TaskReminderDto) { return item.id; }


      onAnimationEnd(event: AnimationEvent) {
            if ((event.target as HTMLElement).classList.contains('closing')) {
                  this.closed.emit();
            }
      }
}
