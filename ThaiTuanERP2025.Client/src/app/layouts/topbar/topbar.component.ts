// topbar.component.ts (đã chỉnh)
import { CommonModule } from '@angular/common';
import { Component, computed, ElementRef, HostListener, inject, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { NotificationPanelService } from './notification-panel/services/notification-panel.service';
import { NotificationStateService } from './notification-panel/services/notification-state.service';
import { NotificationFacade } from './notification-panel/facade/notification.facade';
import { TaskReminderFacade } from './task-reminder-panel/facades/task-reminder.facade';
import { TaskReminderDrawerService } from './task-reminder-panel/services/task-reminder-drawer.service';
import { AuthService } from '../../core/auth/auth.service';
import { Router } from '@angular/router';
import { AvatarUrlPipe } from "../../shared/pipes/avatar-url.pipe";
import { UserFacade } from '../../modules/account/facades/user.facade';
import { UserDto } from '../../modules/account/models/user.model';

@Component({
      selector: 'app-topbar',
      standalone: true,
      imports: [CommonModule, AvatarUrlPipe],
      templateUrl: './topbar.component.html',
      styleUrl: './topbar.component.scss'
})
export class TopbarComponent implements OnInit {
      private readonly userFacade = inject(UserFacade);
      private readonly authService = inject(AuthService); // Giả sử UserFacade có phương thức logout()
      showUserMenu = false;
      private readonly elRef = inject(ElementRef);
      private readonly router = inject(Router);
      
      // Notification
      private readonly notificationFacade = inject(NotificationFacade);
      private readonly notificationPanel = inject(NotificationPanelService);
      private readonly notificationState = inject(NotificationStateService);

      // Task Reminder
      private readonly reminderFacade = inject(TaskReminderFacade);
      private  readonly reminderDrawer = inject(TaskReminderDrawerService);
      readonly reminders = this.reminderFacade.reminders;

      baseUrl: string = environment.baseUrl;
      currentUser$ = this.userFacade.currentUser$;
      currentUser: UserDto | null = null;

      // ❗️Chỉ dùng stream từ Facade — không còn biến cục bộ
      notifications$ = this.notificationFacade.notifications$;
      unreadCount$ = this.notificationFacade.unreadCount$;

      
      readonly activeReminders = computed(() =>
            this.reminders().filter(r => new Date(r.dueAt).getTime() > Date.now())
      );

      async ngOnInit(): Promise<void> {
            await this.notificationFacade.init();
            await this.reminderFacade.init();
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
                  this.reminderDrawer.open(this.reminders, {
                        dismiss: (id) => this.reminderFacade.dismiss(id),
                  });
            }
      }


      // Optional: giữ các hàm này nếu bạn gọi từ nơi khác
      onMarkAllRead() {
            this.notificationFacade.markAllRead();
      }

      onMarkRead(id: string) {
            this.notificationFacade.markRead(id);
      }


      // animation state
      menuVisible = false;
      menuState: 'entering' | 'leaving' | null = null;

      toggleUserMenu(event: MouseEvent) {
            event.stopPropagation();

            if (!this.menuVisible) {
                  this.menuVisible = true;
                  requestAnimationFrame(() => (this.menuState = 'entering'));
            } else {
                  this.startClosingAnimation();
            }
      }
      
      onLogout() {
            this.startClosingAnimation();
            this.authService.logout();
            this.router.navigateByUrl('/login');
      }
      @HostListener('document:click', ['$event'])
      onClickOutside(event: Event) {
            if (this.menuVisible && !this.elRef.nativeElement.contains(event.target)) {
                  this.startClosingAnimation();
            }
      }
      private startClosingAnimation() {
            this.menuState = 'leaving';
            setTimeout(() => {
                  this.menuVisible = false;
                  this.menuState = null;
            }, 150); // phải khớp với duration CSS
      }
}

