import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { combineLatest, interval, map, Observable, startWith } from "rxjs";
import { TaskReminderDto } from "../models/task-reminder.model";
import { Router } from "@angular/router";

@Component({
      selector: 'app-task-reminder-drawer',
      imports: [ CommonModule ],
      templateUrl: './task-reminder-drawer.component.html',
      styleUrls: ['./task-reminder-drawer.component.scss'],
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class TaskReminderDrawerComponent implements OnInit {
      @Input() reminders$!: Observable<TaskReminderDto[]>;
      @Output() dismiss = new EventEmitter<string>();
      @Output() closed = new EventEmitter<void>();

      sortedReminders$!: Observable<TaskReminderDto[]>;   // <-- khai báo, chưa khởi tạo

      constructor(private router: Router) {}

      async ngOnInit(): Promise<void> {
            const timer$ = interval(1000).pipe(startWith(0));
            this.sortedReminders$ = combineLatest([this.reminders$, timer$]).pipe(
                  map(([list]) => {
                        const now = Date.now();
                        return [...list].sort((a, b) => {
                              const aDue = new Date(a.dueAt).getTime();
                              const bDue = new Date(b.dueAt).getTime();
                              const aExpired = aDue <= now;
                              const bExpired = bDue <= now;
                              if (aExpired !== bExpired) return aExpired ? 1 : -1; // còn hạn trước
                              return aDue - bDue; // cùng nhóm: dueAt sớm trước
                        });
                  })
            );
      }

      async goTo(item: TaskReminderDto) {
            const cmdOrUrl = this.resolveRoute(item);
            sessionStorage.setItem('allowPaymentDetailOnce', '1');
            if (Array.isArray(cmdOrUrl)) {
                  await this.router.navigate(cmdOrUrl);           // ví dụ: ['/expense', 'approval-workflow-engine']
            } else {
                  await this.router.navigateByUrl(cmdOrUrl);      // ví dụ: '/expense/approval-workflow-engine'
            }

            // (tuỳ chọn) đóng drawer sau khi điều hướng
            this.dismiss.emit('CLOSE_DRAWER');
      }
      private resolveRoute(tr: TaskReminderDto): (string | number)[] | string {
            if (tr.documentType === 'ExpensePayment' && tr.documentId) {
                  return ['/expense', 'payment-detail', tr.documentId];
            }
            return ['/expense', 'payment-detail'];
      }

      trackById(index: number, item: TaskReminderDto) { return item.id; }

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
      isExpired(item: TaskReminderDto): boolean {
            return new Date(item.dueAt).getTime() <= Date.now();
      }

      onAnimationEnd(event: AnimationEvent) {
            if ((event.target as HTMLElement).classList.contains('closing')) {
                  this.closed.emit();
            }
      }
}
