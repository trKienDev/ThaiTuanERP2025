
import { trigger, transition, query, style, group, animate } from '@angular/animations';
export const loadDomAnimations = trigger('loadDomAnimations', [
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
                        animate('300ms cubic-bezier(0.4, 0, 0.2, 1)', 
                        style({ opacity: 0 }))
                  ], { optional: true }),

                  // fade-in nhẹ khi vào
                  query(':enter', [
                        style({ opacity: 0 }),
                        animate('300ms cubic-bezier(0.22, 1, 0.36, 1)', style({ opacity: 1 }))
                  ], { optional: true }),
            ])
      ]),
]);