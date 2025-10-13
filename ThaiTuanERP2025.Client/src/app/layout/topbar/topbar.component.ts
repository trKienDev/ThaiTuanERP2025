// topbar.component.ts (đã chỉnh)
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { UserFacade } from '../../pages/account/facades/user.facade';
import { UserDto } from '../../pages/account/models/user.model';
import { firstValueFrom, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NotificationPanelService } from './notification-panel/services/notification-panel.service';
import { NotificationStateService } from './notification-panel/services/notification-state.service';
import { NotificationFacade } from './notification-panel/facade/notification.facade';
import { TaskReminderFacade } from './task-reminder-panel/facades/task-reminder.facade';
import { TaskReminderDrawerService } from './task-reminder-panel/services/task-reminder-drawer.service';
import { resolveAvatarUrl } from '../../shared/utils/avatar.utils';

@Component({
      selector: 'app-topbar',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './topbar.component.html',
      styleUrl: './topbar.component.scss'
})
export class TopbarComponent implements OnInit {
      private userFacade = inject(UserFacade);

      // Notification
      private notificationFacade = inject(NotificationFacade);
      private notificationPanel = inject(NotificationPanelService);
      private notificationState = inject(NotificationStateService);

      // Task Reminder
      private reminderFacade = inject(TaskReminderFacade);
      private reminderDrawer = inject(TaskReminderDrawerService);

      baseUrl: string = environment.baseUrl;
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;

      // ❗️Chỉ dùng stream từ Facade — không còn biến cục bộ
      notifications$ = this.notificationFacade.notifications$;
      unreadCount$ = this.notificationFacade.unreadCount$;

      reminders$ = this.reminderFacade.reminders$;
      activeReminders$ = this.reminders$.pipe(
            map(list => list.filter(a => new Date(a.dueAt).getTime() > Date.now()))
      );

      async ngOnInit(): Promise<void> {
            this.currentUser = await firstValueFrom(this.currentUser$);

            // Khởi tạo state: REST + SignalR đã được thực hiện bên trong NotificationStateService
            await this.notificationFacade.init();

            // Khởi tạo nhắc việc
            await this.reminderFacade.init();

            console.log('active reminder: ', await firstValueFrom(this.activeReminders$));
      }

      toggleNotificationPanel(btn: HTMLElement) {
            if (this.notificationPanel.isOpen()) {
                  this.notificationPanel.reposition(btn);
                  this.notificationPanel.updateStreams(this.notifications$, this.unreadCount$, {
                        markAllRead: () => this.notificationState.markAllRead(),
                        markOneRead:  (id) => this.notificationState.markRead(id),
                  });
            } else {
                  this.notificationPanel.open(btn, this.notifications$, this.unreadCount$, {
                        markAllRead: () => this.notificationState.markAllRead(),
                        markOneRead:  (id) => this.notificationState.markRead(id),
                  });
            }
      }

      toggleTaskReminderDrawer() {
            if (this.reminderDrawer.isOpen()) {
                  this.reminderDrawer.close();
            } else {
                  this.reminderDrawer.open(this.reminders$, {
                        dismiss: (id) => this.reminderFacade.dismiss(id),
                  });
            }
      }

      get avatarSrc(): string {
            return resolveAvatarUrl(this.baseUrl, this.currentUser);
      }

      // Optional: giữ các hàm này nếu bạn gọi từ nơi khác
      onMarkAllRead() {
            this.notificationFacade.markAllRead();
      }

      onMarkRead(id: string) {
            this.notificationFacade.markRead(id);
      }
}
