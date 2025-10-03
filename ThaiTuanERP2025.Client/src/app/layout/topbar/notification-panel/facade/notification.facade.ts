// src/app/pages/notifications/facades/notification.facade.ts
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { NotificationStateService } from '../services/notification-state.service';
import { NotificationDto } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationFacade {
      private state = inject(NotificationStateService);

      /** Streams public cho component */
      readonly notifications$: Observable<NotificationDto[]> = this.state.notifications$;
      readonly unreadCount$: Observable<number> = this.state.unreadCount$;

      /** Khởi tạo state (REST + SignalR) */
      async init(): Promise<void> {
            await this.state.init();
      }

      markRead(id: string): void {
            this.state.markRead(id);
      }

      markAllRead(): void {
            this.state.markAllRead();
      }
}
