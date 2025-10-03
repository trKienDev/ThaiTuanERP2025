import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, Input } from "@angular/core";
import { RouterModule } from "@angular/router";
import { NotificationPayload } from "./notification.model";

@Component({
      selector: 'app-notification-panel',
      standalone: true,
      templateUrl: './notification-panel.component.html',
      imports: [ CommonModule, RouterModule ],
      styleUrls: ['./notification-panel.component.scss'],
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class NotificationPanelComponent {
      @Input() notifications: NotificationPayload[] = [];

      onSettings() {
            console.log('Open notification settings');      
      }

      onClickItem(notification: NotificationPayload) {
            if(notification.link) {
                  window.open(notification.link, '_blank');
            }
      }

      formateTime(dateStr?: string | Date): string {
            if (!dateStr) return '';
            const date = typeof dateStr === 'string' ? new Date(dateStr) : dateStr;
            const now = new Date();
            const diff = (now.getTime() - date.getTime()) / 1000;
            if (diff < 60) return 'vừa xong';
            if (diff < 3600) return `${Math.floor(diff / 60)} phút trước`;
            if (diff < 86400) return `${Math.floor(diff / 3600)} giờ trước`;
            return date.toLocaleString();
      }
}