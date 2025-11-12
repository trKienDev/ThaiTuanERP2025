// src/app/shared/services/notification-state.service.ts
import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, firstValueFrom } from 'rxjs';
import { NotificationSignalRService } from './notification-signalr.service';
import { NotificationDto } from '../models/notification.model';
import { NotificationsApiService } from './notification-api.service';
import { ToastNotificationService } from '../../../../shared/components/toast-notification/toast-notification.service';

@Injectable({ providedIn: 'root' })
export class NotificationStateService {
      private readonly api = inject(NotificationsApiService);
      private readonly realtime = inject(NotificationSignalRService);

      private readonly _notifications$ = new BehaviorSubject<NotificationDto[]>([]);
      private readonly _unreadCount$ = new BehaviorSubject<number>(0);

      /** Stream public cho component subscribe */
      readonly notifications$ = this._notifications$.asObservable();
      readonly unreadCount$ = this._unreadCount$.asObservable();

      private readonly notificationToast = inject(ToastNotificationService);

      /** Khởi tạo: load từ REST + start SignalR */
      async init(): Promise<void> {
            // 1. load list & count từ API REST
            const [list, unread] = await Promise.all([
                  firstValueFrom(this.api.getList({ unreadOnly: false, page: 1, pageSize: 30 })),
                  firstValueFrom(this.api.getUnreadCount())
            ]);

            this._notifications$.next(list);
            this._unreadCount$.next(unread);
            
            // 2. start SignalR
            const getToken = () => localStorage.getItem('token');
            await this.realtime.start(getToken);

            // 3. lắng nghe realtime
            this.realtime.incoming$.subscribe(newItems => {
                  const current = this._notifications$.value;
                  const mapped: NotificationDto[] = newItems.map(x => ({ ...x, isRead: false }));
                  const merged: NotificationDto[] = [...mapped, ...current];
                  this._notifications$.next(merged);
                  this._unreadCount$.next(this._unreadCount$.value + (newItems?.length ?? 0));

                  for (const n of mapped) {
                        this.notificationToast.show({
                              id: n.id,                // de-dupe
                              title: n.title || 'Thông báo',
                              message: n.message,
                              link: n.link,
                              duration: 5000
                        });
                  }
            });
      }

      /** Đánh dấu 1 thông báo đã đọc */
      markRead(id: string): void {
            this.api.markRead(id).subscribe(() => {
                  const updated = this._notifications$.value.map(n =>
                        n.id === id ? { ...n, isRead: true } : n
                  );
                  this._notifications$.next(updated);

                  // giảm badge nếu cần
                  const justMarked = updated.find(n => n.id === id)?.isRead === true;
                  if (justMarked) {
                        this._unreadCount$.next(Math.max(0, this._unreadCount$.value - 1));
                  }
            });
      }

      /** Đánh dấu tất cả đã đọc */
      markAllRead(): void {
            this.api.markAllRead().subscribe(() => {
                  const updated = this._notifications$.value.map(n => ({ ...n, isRead: true }));
                  this._notifications$.next(updated);
                  this._unreadCount$.next(0);
                  this.realtime.markAllAsRead(); // reset client counter
            });
      }
}
