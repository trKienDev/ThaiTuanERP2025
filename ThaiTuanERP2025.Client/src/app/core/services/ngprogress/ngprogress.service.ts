import { Injectable } from '@angular/core';
import NProgress from 'nprogress';

@Injectable({ providedIn: 'root' })
export class ProgressService {
      constructor() {
            NProgress.configure({
                  showSpinner: false,   // không hiển thị spinner tròn
                  trickleSpeed: 100,    // tốc độ “chảy” của thanh progress
                  minimum: 0.1,
                  easing: 'ease',
                  speed: 500,           // thời gian hoàn tất
            });
      }

      start() {
            NProgress.start();
      }

      done() {
            NProgress.done();
      }
}
