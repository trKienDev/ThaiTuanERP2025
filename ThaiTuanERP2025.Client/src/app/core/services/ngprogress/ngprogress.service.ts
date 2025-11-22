import { Injectable } from '@angular/core';
import NProgress from 'nprogress';

@Injectable({ providedIn: 'root' })
export class ProgressService {
      constructor() {
            NProgress.configure({
                  showSpinner: false,   // không hiển thị spinner tròn
                  trickleSpeed: 150,    // tốc độ “chảy” của thanh progress
                  minimum: 0.3,
                  easing: 'ease',
                  speed: 50,           // thời gian hoàn tất
            });
      }

      start() {
            NProgress.start();
      }

      done() {
            NProgress.done();
      }
}
