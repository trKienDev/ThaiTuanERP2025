import { Directive, Input } from "@angular/core";

@Directive()
export abstract class KitBaseIcon {
      @Input() size: string | number = 20;

      get computedSize(): string {
            return typeof this.size === 'number' ? `${this.size}px` : this.size;
      }
}