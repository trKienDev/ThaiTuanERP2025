import { Injectable, OnDestroy } from "@angular/core";
import { Subscription, interval } from "rxjs";
import { AuthService } from "./auth.service";

@Injectable({ providedIn: 'root' })
export class RefreshScheduler implements OnDestroy {
      private refreshSub?: Subscription;

      constructor(private authService: AuthService) {}

      /**
       * Khởi động bộ kiểm tra token định kỳ
       */
      start(intervalMs: number = 60_000) {
            // Dừng nếu đang chạy
            this.stop();

            console.log('[RefreshScheduler] ⏳ Started checking token every', intervalMs / 1000, 'seconds');

            this.refreshSub = interval(intervalMs).subscribe(() => {
                  const token = this.authService.getToken();
                  if (!token) return;

                  try {
                        const payload = JSON.parse(atob(token.split('.')[1]));
                        const exp = payload.exp;
                        const now = Math.floor(Date.now() / 1000);
                        const remaining = exp - now;

                        // Nếu còn ít hơn 60 giây → refresh
                        if (remaining < 60 && remaining > 0) {
                              console.log(`[RefreshScheduler] 🔄 Token expiring in ${remaining}s → refreshing`);
                              this.authService.refreshToken().subscribe();
                        }
                  } catch {
                        console.warn('[RefreshScheduler] Invalid token payload');
                  }
            });
      }

      /**
       * Dừng kiểm tra
       */
      stop() {
            if (this.refreshSub) {
                  this.refreshSub.unsubscribe();
                  this.refreshSub = undefined;
                  console.log('[RefreshScheduler] ⏹️ Stopped');
            }
      }

      ngOnDestroy() {
            this.stop();
      }
}