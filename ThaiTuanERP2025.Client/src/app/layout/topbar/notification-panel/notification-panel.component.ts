import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from "@angular/core";
import { RouterModule } from "@angular/router";
import { NotificationDto, NotificationPayload } from "./models/notification.model";
import { Observable } from "rxjs";

@Component({
      selector: 'app-notification-panel',
      standalone: true,
      templateUrl: './notification-panel.component.html',
      imports: [ CommonModule, RouterModule ],
      styleUrls: ['./notification-panel.component.scss'],
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotificationPanelComponent {
      /** Streams từ state service */
      @Input() notifications$!: Observable<NotificationDto[]>;
      @Input() unreadCount$!: Observable<number>;

      /** Sự kiện để overlay/service bên ngoài bắt */
      @Output() markAllRead = new EventEmitter<void>();
      @Output() markOneRead = new EventEmitter<string>();

      onSettings() {
      // optional: điều hướng trang cài đặt
            console.log('open notification settings');
      }

      onClickItem(n: NotificationDto) {
            // Điều hướng nếu có link
            if (n.link) window.open(n.link, '_blank');
      }

      onMarkOne(ev: MouseEvent, id: string) {
            ev.stopPropagation();
            this.markOneRead.emit(id);
      }

      trackById = (_: number, n: NotificationDto) => n.id;

      formatTime(dt?: string | Date): string {
            if (!dt) return '';
            const d = typeof dt === 'string' ? new Date(dt) : dt;
            const now = new Date();
            const diff = (now.getTime() - d.getTime()) / 1000;
            if (diff < 60) return 'vừa xong';
            if (diff < 3600) return `${Math.floor(diff / 60)} phút trước`;
            if (diff < 86400) return `${Math.floor(diff / 3600)} giờ trước`;
            return d.toLocaleString();
      }
}