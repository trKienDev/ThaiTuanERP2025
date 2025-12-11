import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './core/auth/auth.service';
import { RefreshScheduler } from './core/auth/refresh.scheduler';
import { Subscription } from 'rxjs';

@Component({
      selector: 'app-root',
      standalone: true,
      imports: [ RouterOutlet ],
      templateUrl: './app.component.html',
      styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
      title = 'ThaiTuanERP2025.Client';
      private sub?: Subscription;

      constructor(
            private readonly authService: AuthService, private readonly refreshScheduler: RefreshScheduler,
      ) {}

      ngOnInit() {
            this.authService.checkTokenValidity();
            this.authService.initAfterReload();
            this.sub = this.authService.currentUser$.subscribe(user => {
                  if (user) {
                        this.refreshScheduler.start(); // Bắt đầu scheduler khi login
                  } else {
                        this.refreshScheduler.stop(); // Dừng scheduler khi logout
                  }
            });
      }
}
