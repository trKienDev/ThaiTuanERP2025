// shared/notifications/notification-panel.service.ts
import { ComponentRef, Injectable, Injector } from '@angular/core';
import { Overlay, OverlayRef, ConnectedPosition, FlexibleConnectedPositionStrategy, OverlayConfig } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { NotificationPanelComponent } from './notification-panel.component';
import { NotificationPayload } from './notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationPanelService {
      private overlayRef?: OverlayRef;
      private compRef?: ComponentRef<NotificationPanelComponent>;

      constructor(private overlay: Overlay, private injector: Injector) {}

      isOpen(): boolean {
            return !!this.overlayRef && this.overlayRef.hasAttached();
      }

      open(origin: HTMLElement, notifications: NotificationPayload[]) {
            if (this.isOpen()) {
                  this.update(notifications);
                  this.overlayRef!.updatePositionStrategy(
                        this.overlay.position().flexibleConnectedTo(origin).withPositions(this.positions())
                  );
                  return;
            }

            const positions: ConnectedPosition[] = [
                  { originX: 'end', originY: 'bottom', overlayX: 'end', overlayY: 'top', offsetY: 8 },
                  { originX: 'end', originY: 'top',    overlayX: 'end', overlayY: 'bottom', offsetY: -8 },
            ];

            const positionStrategy: FlexibleConnectedPositionStrategy = this.overlay.position()
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
            this.compRef = this.overlayRef.attach(portal);

            // bơm dữ liệu initial
            this.compRef.instance.notifications = notifications;
            this.compRef.changeDetectorRef.markForCheck();

            this.overlayRef.backdropClick().subscribe(() => this.close());
            this.overlayRef.detachments().subscribe(() => this.dispose());
      }

      update(notifications: NotificationPayload[]) {
            if (!this.compRef || !this.overlayRef?.hasAttached()) return;
            this.compRef.instance.notifications = notifications;
            this.compRef.changeDetectorRef.markForCheck();
      }

      /** Reposition thủ công (nếu cần) */
      reposition(origin: HTMLElement) {
            if (!this.overlayRef) return;
                  this.overlayRef.updatePositionStrategy(
                  this.overlay.position().flexibleConnectedTo(origin).withPositions(this.positions())
            );
            this.overlayRef.updatePosition();
      }

      close() {
            this.overlayRef?.detach();
            this.dispose();
      }

      private dispose() {
            this.overlayRef?.dispose();
            this.overlayRef = undefined;
            this.compRef = undefined;
      }

      private positions(): ConnectedPosition[] {
            return [
                  { originX: 'end', originY: 'bottom', overlayX: 'end', overlayY: 'top', offsetY: 8 },
                  { originX: 'end', originY: 'top',    overlayX: 'end', overlayY: 'bottom', offsetY: -8 },
            ];
      }
}
