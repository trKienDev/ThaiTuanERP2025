import { Injectable } from '@angular/core';
import { BehaviorSubject, map, shareReplay } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CountdownService {
      // ONE shared stream for the entire app
      private readonly tick$ = new BehaviorSubject(Date.now());

      constructor() {
            // Global interval: chạy duy nhất 1 lần cho toàn app
            setInterval(() => {
                  this.tick$.next(Date.now());
            }, 1000);
      }

      /**
       * Trả về countdown (seconds remaining)
       */
      createCountdown(targetTime: Date) {
            const target = targetTime.getTime();

            return this.tick$.pipe(
                  map(now => Math.max(0, Math.floor((target - now) / 1000))),
                  shareReplay(1) // Cache current value để không re-calc
            );
      }
}
