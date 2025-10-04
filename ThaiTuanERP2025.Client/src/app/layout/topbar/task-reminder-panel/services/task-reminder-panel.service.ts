// src/app/shared/alarms/alarm-panel.service.ts
import { Injectable, Injector, ComponentRef } from '@angular/core';
import { Overlay, OverlayRef, ConnectedPosition, FlexibleConnectedPositionStrategy, OverlayConfig } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Observable, Subscription } from 'rxjs';
import { TaskReminderPanelComponent } from '../task-reminder-panel.component';
import { TaskReminderDto } from '../models/task-reminder.model';

export interface TaskReminderPanelHandlers {
      dismiss?: (id: string) => void;
}

@Injectable({ providedIn: 'root' })
export class TaskReminderPanelService {
      private overlayRef?: OverlayRef;
      private compRef?: ComponentRef<TaskReminderPanelComponent>;
      private subs = new Subscription();

      constructor(private overlay: Overlay, private injector: Injector) {}

      isOpen(): boolean {
            return !!this.overlayRef && this.overlayRef.hasAttached();
      }

      open(origin: HTMLElement, reminders$: Observable<TaskReminderDto[]>, handlers?: TaskReminderPanelHandlers) {
            if (this.isOpen()) {
                  this.reposition(origin);
                  this.updateStreams(reminders$, handlers);
                  return;
            }

            const positionStrategy: FlexibleConnectedPositionStrategy = this.overlay
                  .position()
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
            const portal = new ComponentPortal(TaskReminderPanelComponent, null, this.injector);
            this.compRef = this.overlayRef.attach(portal);

            this.compRef.instance.reminders$ = reminders$;

            this.wireHandlers(handlers);

            this.overlayRef.backdropClick().subscribe(() => this.close());
            this.overlayRef.detachments().subscribe(() => this.dispose());
      }

      updateStreams(reminders$: Observable<TaskReminderDto[]>, handlers?: TaskReminderPanelHandlers) {
            if (!this.compRef || !this.overlayRef?.hasAttached()) return;
            this.compRef.instance.reminders$ = reminders$;
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

      private wireHandlers(handlers?: TaskReminderPanelHandlers, reset = false) {
            if (!this.compRef) return;
            if (reset) {
                  this.subs.unsubscribe();
                  this.subs = new Subscription();
            }
            if (handlers?.dismiss) {
                  this.subs.add(this.compRef.instance.dismiss.subscribe(id => handlers.dismiss!(id)));
            }
      }

      private positions(): ConnectedPosition[] {
            return [
                  { originX: 'end', originY: 'bottom', overlayX: 'end', overlayY: 'top', offsetY: 8 },
                  { originX: 'end', originY: 'top', overlayX: 'end', overlayY: 'bottom', offsetY: -8 }
            ];
      }
}
