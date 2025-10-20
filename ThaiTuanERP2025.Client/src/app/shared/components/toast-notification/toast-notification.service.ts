import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type ToastPosition = 'bottom-right' | 'top';

export interface ToastNotification {
      id: string;                  // nên set = notification.id để de-dupe
      title?: string;
      message?: string;
      link?: string;               // nếu có → click sẽ open link
      duration?: number;           // ms, default 5000
      createdAt?: Date;
}

@Injectable({ providedIn: 'root' })
export class ToastNotificationService {
      private _queue$ = new BehaviorSubject<ToastNotification[]>([]);
      readonly queue$ = this._queue$.asObservable();

      /** tối đa số toast hiển thị cùng lúc */
      private readonly MAX_CONCURRENT = 1;

      /** push toast, de-dupe theo id nếu có */
      show(toast: ToastNotification) {
            const duration = toast.duration ?? 5000;
            const id = toast.id ?? crypto.randomUUID();
            const item: ToastNotification = { ...toast, id, duration, createdAt: new Date() };

            const list = this._queue$.value;
            // de-dupe: nếu đã có cùng id, bỏ qua
            if (id && list.some(t => t.id === id)) return;

            // chỉ giữ tối đa MAX_CONCURRENT ở đầu (mới nhất trên cùng)
            const next = [item, ...list].slice(0, this.MAX_CONCURRENT);
            this._queue$.next(next);
      }

      dismiss(id: string) {
            this._queue$.next(this._queue$.value.filter(t => t.id !== id));
      }

      clear() {
            this._queue$.next([]);
      }
}
