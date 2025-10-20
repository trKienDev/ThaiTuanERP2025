import { Component } from '@angular/core';
import { TopbarComponent } from '../topbar/topbar.component';
import { CommonModule } from '@angular/common';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router, RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { ToastNotificationHostComponent } from "../../shared/components/toast-notification/toast-notification-host.component";
import { loadDomAnimations } from '../../shared/animations/load-dom/load-dom.animation';
import { ProgressService } from '../../core/services/ngprogress/ngprogress.service';

@Component({
      selector: 'app-layout-shell',
      standalone: true,
      imports: [CommonModule, RouterOutlet, SidebarComponent, TopbarComponent, ToastNotificationHostComponent],
      templateUrl: './layout-shell.component.html',
      styleUrls: ['./layout-shell.component.scss'],
      animations: [loadDomAnimations]
})
export class LayoutShellComponent {
      constructor(private router: Router, private progress: ProgressService) {
            this.router.events.subscribe(event => {
                  if (event instanceof NavigationStart) {
                        this.progress.start();
                  } else if (
                        event instanceof NavigationEnd ||
                        event instanceof NavigationCancel ||
                        event instanceof NavigationError
                  ) {
                        this.progress.done();
                  }
            });
      }
}
