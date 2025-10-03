import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { UserFacade } from '../../pages/account/facades/user.facade';
import { UserDto } from '../../pages/account/models/user.model';
import { firstValueFrom, takeUntil } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NotificationSignalRService } from '../../core/services/realtime/notification-signalr.service';
import { Subject } from 'rxjs';
import { NotificationPayload } from './notification-panel/notification.model';
import { NotificationPanelService } from './notification-panel/notification-panel.service';

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
      private notificationPanel = inject(NotificationPanelService);

      private destroy$ = new Subject<void>();

      baseUrl: string = environment.baseUrl;      
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;

      notifications: NotificationPayload[] = [];
      unreadCount = 0;

      async ngOnInit(): Promise<void> {
            this.currentUser = await firstValueFrom(this.currentUser$);

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

      togglePanel(btn: HTMLElement) {
            if(this.notificationPanel.isOpen()) {
                  this.notificationPanel.close();
            } else {
                  this.notificationPanel.open(btn, this.notifications);
                  this.notifierService.markAllAsRead();
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
}
