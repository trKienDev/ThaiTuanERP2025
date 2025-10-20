import { Directive, ElementRef, HostListener, AfterViewInit } from '@angular/core';

@Directive({
      selector: 'textarea[autoResize]',
      standalone: true
})
export class AutoResizeDirective implements AfterViewInit {
      constructor(private el: ElementRef<HTMLTextAreaElement>) {}

      ngAfterViewInit() {
            this.resize();
      }

      @HostListener('input')
            onInput() {
            this.resize();
      }

      private resize() {
            const ta = this.el.nativeElement;
            ta.style.height = 'auto';
            ta.style.overflow = 'hidden';
            ta.style.height = `${ta.scrollHeight}px`;
      }
}
