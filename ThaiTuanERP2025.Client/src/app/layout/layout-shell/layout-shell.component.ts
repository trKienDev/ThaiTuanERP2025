import { Component } from '@angular/core';
import { TopbarComponent } from '../topbar/topbar.component';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { ToastNotificationHostComponent } from "../../shared/components/toast-notification/toast-notification-host.component";
import { trigger, transition, query, style, group, animate } from '@angular/animations';

@Component({
      selector: 'app-layout-shell',
      standalone: true,
      imports: [CommonModule, RouterOutlet, SidebarComponent, TopbarComponent, ToastNotificationHostComponent],
      templateUrl: './layout-shell.component.html',
      styleUrls: ['./layout-shell.component.scss'],
      animations: [
            trigger('routeAnimations', [
                  transition('* <=> *', [
                        query(':enter, :leave', [
                              style({
                                    position: 'absolute',
                                    width: '100%',
                                    opacity: 0,
                              })
                        ], { optional: true }),

                        group([
                              // fade-out nhẹ khi rời khỏi
                              query(':leave', [
                                    animate('200ms ease', style({ opacity: 0 }))
                              ], { optional: true }),

                              // fade-in nhẹ khi vào
                              query(':enter', [
                                    style({ opacity: 0 }),
                                    animate('400ms ease-out', style({ opacity: 1 }))
                              ], { optional: true }),
                        ])
                  ]),
            ]),
      ],
})
export class LayoutShellComponent {
      
}
