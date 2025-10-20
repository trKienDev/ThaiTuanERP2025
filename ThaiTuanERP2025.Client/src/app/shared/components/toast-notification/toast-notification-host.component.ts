import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy } from '@angular/core';
import { ToastNotificationService } from './toast-notification.service';
import { Subscription, timer } from 'rxjs';

@Component({
      selector: 'app-toast-host',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './toast-notification-host.component.html',
      styleUrls: ['./toast-notification-host.component.scss']
})
export class ToastNotificationHostComponent implements OnDestroy {
      private toast = inject(ToastNotificationService);
      queue$ = this.toast.queue$;

      /** quản lý timer tự ẩn theo id */
      private timers = new Map<string, { sub: Subscription; remaining: number; startedAt: number }>();

      /** gọi khi toast render lần đầu */
      onMount(id: string, duration: number) {
            // nếu đã có timer (re-render) thì bỏ qua
            if (this.timers.has(id)) return;
            this.startTimer(id, duration);
      }

      startTimer(id: string, duration: number) {
            const sub = timer(duration).subscribe(() => this.toast.dismiss(id));
            this.timers.set(id, { sub, remaining: duration, startedAt: Date.now() });
      }

      pauseTimer(id: string) {
            const t = this.timers.get(id);
            if (!t) return;
            t.sub.unsubscribe();
            const elapsed = Date.now() - t.startedAt;
            t.remaining = Math.max(0, t.remaining - elapsed);
            this.timers.set(id, t);
      }

      resumeTimer(id: string) {
            const t = this.timers.get(id);
            if (!t) return;
            const sub = timer(t.remaining).subscribe(() => this.toast.dismiss(id));
            t.sub = sub;
            t.startedAt = Date.now();
            this.timers.set(id, t);
      }

      onClick(id: string, link?: string) {
            if (link) window.open(link, '_blank');
            this.toast.dismiss(id);
      }

      onClose(id: string) {
            this.toast.dismiss(id);
      }

      trackById = (_: number, t: any) => t.id;

      ngOnDestroy(): void {
            // cleanup tất cả timers
            this.timers.forEach(t => t.sub.unsubscribe());
            this.timers.clear();
      }
}
