
import {
  trigger,
  transition,
  query,
  style,
  group,
  animate
} from '@angular/animations';

/**
 * Premium Fade + Soft Blur Route Animation
 * -----------------------------------------
 * Hiệu ứng này mô phỏng cảm giác mượt như ứng dụng desktop (Slack, Outlook, Teams).
 * Khi chuyển route, view cũ sẽ mờ dần, mờ nhẹ (blur), rồi view mới hiện dần lên trong nền sáng mịn.
 * 
 * Đặc điểm:
 *  - Fade mượt 500ms, easing vật lý “easeOutQuart”.
 *  - Nền blur nhẹ 6px, tan dần về 0 khi view mới xuất hiện.
 *  - Không thay đổi vị trí, không trượt.
 *  - Áp dụng được cho mọi cấp router-outlet (layout-shell, expense, finance, …).
 */
export const loadDomAnimations = trigger('loadDomAnimations', [
      transition('* <=> *', [
            query(':enter, :leave', [
                  style({
                        position: 'absolute',
                        width: 'fir-content',
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
]);