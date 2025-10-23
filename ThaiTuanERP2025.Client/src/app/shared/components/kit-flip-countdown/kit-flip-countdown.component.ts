import { CommonModule } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';

@Component({
      selector: 'kit-flip-countdown',
      standalone: true,
      imports: [CommonModule],
      templateUrl: './kit-flip-countdown.component.html',
      styleUrls: ['./kit-flip-countdown.component.scss'],
      animations: [
            trigger('slideDigit', [
                  transition(':increment, :decrement', [
                        style({ transform: 'translateY(100%)', opacity: 0 }),
                        animate(
                              '400ms cubic-bezier(0.25, 0.1, 0.25, 1)',
                              style({ transform: 'translateY(0)', opacity: 1 })
                        )
                  ])
            ])
      ]
})
export class KitFlipCountdownComponent implements OnInit, OnDestroy {
      @Input() timeLeft: number = 0;

      hoursTens = 0;
      hoursOnes = 0;
      minutesTens = 0;
      minutesOnes = 0;
      secondsTens = 0;
      secondsOnes = 0;

      prev = { hoursOnes: 0, minutesOnes: 0, secondsOnes: 0 };

      private timerId?: any;

      ngOnInit() {
            this.updateDigits();
            this.startCountdown();
      }

      ngOnDestroy() {
            clearInterval(this.timerId);
      }

      private startCountdown() {
            this.timerId = setInterval(() => {
                  if (this.timeLeft > 0) {
                        this.timeLeft--;
                        this.updateDigits();
                  } else {
                        clearInterval(this.timerId);
                  }
            }, 1000);
      }

      private updateDigits() {
            const total = Math.max(0, Math.floor(this.timeLeft));

            const hours = Math.floor(total / 3600);
            const minutes = Math.floor((total % 3600) / 60);
            const seconds = total % 60;

            this.hoursTens = Math.floor(hours / 10);
            this.hoursOnes = hours % 10;

            this.minutesTens = Math.floor(minutes / 10);
            this.minutesOnes = minutes % 10;

            this.prev = {
                  hoursOnes: this.hoursOnes,
                  minutesOnes: this.minutesOnes,
                  secondsOnes: this.secondsOnes
            };

            this.secondsTens = Math.floor(seconds / 10);
            this.secondsOnes = seconds % 10;
      }
}
