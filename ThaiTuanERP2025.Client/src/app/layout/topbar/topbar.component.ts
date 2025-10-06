import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { UserFacade } from '../../pages/account/facades/user.facade';
import { UserDto } from '../../pages/account/models/user.model';
import { firstValueFrom, map, take, takeUntil } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Subject } from 'rxjs';
import { NotificationSignalRService } from './notification-panel/services/notification-signalr.service';
import { NotificationPanelService } from './notification-panel/services/notification-panel.service';
import { NotificationStateService } from './notification-panel/services/notification-state.service';
import { NotificationFacade } from './notification-panel/facade/notification.facade';
import { NotificationDto } from './notification-panel/models/notification.model';
import { TaskReminderFacade } from './task-reminder-panel/facades/task-reminder.facade';
import { TaskReminderDrawerService } from './task-reminder-panel/services/task-reminder-drawer.service';

@Component({
      selector: 'app-topbar',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './topbar.component.html',
      styleUrl: './topbar.component.scss'
})
export class TopbarComponent implements OnInit {
      private userFacade = inject(UserFacade);
      private notifierService = inject(NotificationSignalRService);
      private notificationFacade = inject(NotificationFacade);
      private notificationPanel = inject(NotificationPanelService);
      private notificationState = inject(NotificationStateService);

      private reminderFacade = inject(TaskReminderFacade);
      private reminderDrawer = inject(TaskReminderDrawerService);

      private destroy$ = new Subject<void>();

      baseUrl: string = environment.baseUrl;      
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;

      notifications: NotificationDto[] = [];
      unreadCount = 0;
      notifications$ = this.notificationFacade.notifications$;
      unreadCount$ = this.notificationFacade.unreadCount$;

      reminders$ = this.reminderFacade.reminders$;
      activeReminders$ = this.reminders$.pipe(
            map(list => list.filter(a => new Date(a.dueAt).getTime() > Date.now()))
      );

      async ngOnInit(): Promise<void> {
            this.currentUser = await firstValueFrom(this.currentUser$);
            await this.notificationFacade.init();
            await this.reminderFacade.init();

            const getToken = () => localStorage.getItem('access_token');

            this.notifierService.start(getToken);

            this.notifierService.incoming$
                  .pipe(takeUntil(this.destroy$))
                  .subscribe(payloads => {
                        // thêm vào đầu danh sách (mới nhất trên cùng)
                        this.notifications = [...payloads.map(p => ({ ...p, unread: true })), ...this.notifications];
                  });

            this.notifierService.unreadCount$
                  .pipe(takeUntil(this.destroy$))
                  .subscribe(count => {
                        this.unreadCount = count;
                  });
      }

      toggleNotificationPanel(btn: HTMLElement) {
            if(this.notificationPanel.isOpen()) {
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
            if(this.reminderDrawer.isOpen()) {
                  this.reminderDrawer.close();
            } else {
                  this.reminderDrawer.open(this.reminders$, {
                        dismiss: (id) => this.reminderFacade.dismiss(id),
                  });
            }
      }

      ngOnDestroy(): void {
            this.destroy$.next();
            this.destroy$.complete();
      }

      get avatarSrc(): string {
            if (this.currentUser?.avatarFileId && this.currentUser.avatarFileId.startsWith('data:image')) {
                  return this.currentUser.avatarFileId; // base64 preview
            }
            if (this.currentUser?.avatarFileObjectKey) {
                  return this.baseUrl + '/files/public/' + this.currentUser.avatarFileObjectKey;
            }
            return 'default-user-avatar.jpg';
      }

      onNotificationsClick() {
            this.notifierService.markAllAsRead();
      }

      onMarkAllRead() {
            this.notificationFacade.markAllRead();
      }

      onMarkRead(id: string) {
            this.notificationFacade.markRead(id);
      }
}