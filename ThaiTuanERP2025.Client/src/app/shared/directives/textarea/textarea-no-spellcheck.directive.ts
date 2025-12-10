import { Directive, ElementRef, AfterViewInit } from '@angular/core';

@Directive({
      selector: 'textarea[noSpellcheck], input[type="text"][noSpellcheck]',
      standalone: true
})
export class TextareaNoSpellcheckDirective implements AfterViewInit {
      constructor(private el: ElementRef<HTMLInputElement | HTMLTextAreaElement>) {}

      ngAfterViewInit() {
            this.el.nativeElement.spellcheck = false;
      }
}
