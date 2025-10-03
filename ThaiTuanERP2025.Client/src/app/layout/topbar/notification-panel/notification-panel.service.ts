// shared/notifications/notification-panel.service.ts
import { Injectable, Injector } from '@angular/core';
import { Overlay, OverlayRef, ConnectedPosition, FlexibleConnectedPositionStrategy, OverlayConfig } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { NotificationPanelComponent } from './notification-panel.component';
import { NotificationPayload } from './notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationPanelService {
  private overlayRef?: OverlayRef;

  constructor(private overlay: Overlay, private injector: Injector) {}

  isOpen(): boolean {
    return !!this.overlayRef && this.overlayRef.hasAttached();
  }

  open(origin: HTMLElement, notifications: NotificationPayload[]) {
    if (this.isOpen()) {
      this.close();
    }

    const positions: ConnectedPosition[] = [
      { originX: 'end', originY: 'bottom', overlayX: 'end', overlayY: 'top', offsetY: 8 },
      { originX: 'end', originY: 'top',    overlayX: 'end', overlayY: 'bottom', offsetY: -8 },
    ];

    const positionStrategy: FlexibleConnectedPositionStrategy = this.overlay
      .position()
      .flexibleConnectedTo(origin)
      .withPositions(positions)
      .withPush(true);

    const config: OverlayConfig = {
      positionStrategy,
      hasBackdrop: true,
      backdropClass: 'cdk-overlay-transparent-backdrop',
      scrollStrategy: this.overlay.scrollStrategies.reposition()
    };

    this.overlayRef = this.overlay.create(config);
    const portal = new ComponentPortal(NotificationPanelComponent, null, this.injector);
    const compRef = this.overlayRef.attach(portal);
    compRef.instance.notifications = notifications;

    this.overlayRef.backdropClick().subscribe(() => this.close());
    this.overlayRef.detachments().subscribe(() => this.dispose());
  }

  update(notifications: NotificationPayload[]) {
    if (!this.overlayRef?.hasAttached()) return;
    const comp = this.overlayRef!.hostElement.querySelector('app-notification-panel');
    // cách an toàn hơn: lấy instance từ attach lúc open:
    // ở ví dụ bên trên, ta đã giữ compRef.instance.notifications = notifications
    // nên nếu cần update, bạn nên lưu compRef ở private và set lại:
    // this.compRef?.instance.notifications = notifications;
    // Để đơn giản, bạn có thể close và open lại:
    this.close();
  }

  close() {
    this.overlayRef?.detach();
  }

  private dispose() {
    this.overlayRef?.dispose();
    this.overlayRef = undefined;
  }
}
