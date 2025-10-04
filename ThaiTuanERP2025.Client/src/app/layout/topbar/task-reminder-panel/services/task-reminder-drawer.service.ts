// src/app/shared/task-reminder-drawer.service.ts
import { Injectable, Injector, ComponentRef } from '@angular/core';
import { Overlay, OverlayConfig, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Observable, Subscription } from 'rxjs';
import { TaskReminderDrawerComponent } from '../drawer/task-reminder-drawer.component';
import { TaskReminderDto } from '../models/task-reminder.model';

@Injectable({ providedIn: 'root' })
export class TaskReminderDrawerService {
      private overlayRef?: OverlayRef;
      private compRef?: ComponentRef<TaskReminderDrawerComponent>;
      private subs = new Subscription();

      constructor(private overlay: Overlay, private injector: Injector) {}

      isOpen(): boolean {
            return !!this.overlayRef && this.overlayRef.hasAttached();
      }

      open(reminders$: Observable<TaskReminderDto[]>, handlers?: { dismiss?: (id: string) => void }) {
            if (this.isOpen()) {
                  this.updateStreams(reminders$, handlers);
                  return;
            }

            const config: OverlayConfig = {
                  hasBackdrop: true,
                  backdropClass: 'cdk-overlay-dark-backdrop',
                  scrollStrategy: this.overlay.scrollStrategies.block(),
                  positionStrategy: this.overlay.position().global().right('0').top('0').bottom('0')
            };

            this.overlayRef = this.overlay.create(config);
            this.compRef = this.overlayRef.attach(new ComponentPortal(TaskReminderDrawerComponent, null, this.injector));

            this.compRef.instance.reminders$ = reminders$;

            if (handlers?.dismiss) {
                  this.subs.add(this.compRef.instance.dismiss.subscribe(id => {
                        if (id === 'CLOSE_DRAWER') this.close();
                        else handlers.dismiss!(id);
                  }));
            }

            this.overlayRef.backdropClick().subscribe(() => this.close());
            this.overlayRef.detachments().subscribe(() => this.dispose());
      }

      updateStreams(reminders$: Observable<TaskReminderDto[]>, handlers?: { dismiss?: (id: string) => void }) {
            if (!this.compRef || !this.overlayRef?.hasAttached()) return;
            this.compRef.instance.reminders$ = reminders$;
      }

      close() {
            if (!this.compRef) return;
            const el = this.compRef.location.nativeElement.querySelector('.drawer');
            if (el) {
                  el.classList.add('closing');
                  // lắng nghe closed event từ component
                  this.subs.add(this.compRef.instance.closed.subscribe(() => {
                        this.overlayRef?.detach();
                        this.dispose();
                  }));
            } else {
                  // fallback nếu không tìm thấy element
                  this.overlayRef?.detach();
                  this.dispose();
            }
      }

      private dispose() {
            this.subs.unsubscribe();
            this.subs = new Subscription();
            this.compRef?.destroy();
            this.compRef = undefined;
            this.overlayRef?.dispose();   
            this.overlayRef = undefined;
      }
}
