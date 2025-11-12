// src/app/shared/notifications/notification-panel.service.ts
import { Injectable, Injector, ComponentRef } from '@angular/core';
import { Overlay, OverlayRef, ConnectedPosition, FlexibleConnectedPositionStrategy, OverlayConfig } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Observable, Subscription } from 'rxjs';
import { NotificationPanelComponent } from '../notification-panel.component';
import { NotificationDto } from '../models/notification.model';

export interface NotificationPanelHandlers {
      markAllRead?: () => void;
      markOneRead?: (id: string) => void;
}

@Injectable({ providedIn: 'root' })
export class NotificationPanelService {
      private overlayRef?: OverlayRef;
      private compRef?: ComponentRef<NotificationPanelComponent>;
      private subs = new Subscription();

      constructor(private readonly overlay: Overlay, private readonly injector: Injector) {}

      isOpen(): boolean {
            return !!this.overlayRef && this.overlayRef.hasAttached();
      }

      open(origin: HTMLElement, notifications$: Observable<NotificationDto[]>, unreadCount$: Observable<number>, handlers?: NotificationPanelHandlers) {
            if (this.isOpen()) {
                  this.reposition(origin);
                  this.updateStreams(notifications$, unreadCount$, handlers);
                  return;
            }

            const positionStrategy: FlexibleConnectedPositionStrategy = this.overlay.position()
                  .flexibleConnectedTo(origin)
                  .withPositions(this.positions())
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

            // Gắn streams
            this.compRef.instance.notifications$ = notifications$;
            this.compRef.instance.unreadCount$ = unreadCount$;

            // Gắn handlers
            this.wireHandlers(handlers);

            // Close khi click backdrop
            this.overlayRef.backdropClick().subscribe(() => this.close());
            this.overlayRef.detachments().subscribe(() => this.dispose());
      }

      updateStreams( notifications$: Observable<NotificationDto[]>, unreadCount$: Observable<number>, handlers?: NotificationPanelHandlers) {
            if (!this.compRef || !this.overlayRef?.hasAttached()) return;
            this.compRef.instance.notifications$ = notifications$;
            this.compRef.instance.unreadCount$ = unreadCount$;
            this.wireHandlers(handlers, true);
            this.compRef.changeDetectorRef.markForCheck();
      }

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
            this.subs.unsubscribe();
            this.subs = new Subscription();
            this.compRef?.destroy();
            this.compRef = undefined;
            this.overlayRef?.dispose();
            this.overlayRef = undefined;
      }

      private wireHandlers(handlers?: NotificationPanelHandlers, reset = false) {
            if (!this.compRef) return;
            if (reset) {
                  this.subs.unsubscribe();
                  this.subs = new Subscription();
            }
            if (handlers?.markAllRead) {
                  this.subs.add(this.compRef.instance.markAllRead.subscribe(() => handlers.markAllRead!()));
            }
            if (handlers?.markOneRead) {
                  this.subs.add(this.compRef.instance.markOneRead.subscribe(id => handlers.markOneRead!(id)));
            }
      }

      private positions(): ConnectedPosition[] {
            return [
                  { originX: 'end', originY: 'bottom', overlayX: 'end', overlayY: 'top', offsetY: 8 },
                  { originX: 'end', originY: 'top',    overlayX: 'end', overlayY: 'bottom', offsetY: -8 },
            ];
      }
}
