import { ConnectedPosition, OverlayModule } from "@angular/cdk/overlay";
import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { ActionMenuOption } from "./kit-action-menu.model";

@Component({
      selector: 'kit-action-menu',
      standalone: true,
      imports: [ CommonModule, OverlayModule ],
      templateUrl: './kit-action-menu.component.html',
      styleUrl: './kit-action-menu.component.scss'
})
export class KitActionMenuComponent {
      @Input() actions: ActionMenuOption[] = [];
      @Input() icon: string = 'more_horiz';

      menuOpenIndex: number | null = null;
      menuOverlayPosition: ConnectedPosition[] = [
            { originX: 'center', originY: 'bottom', overlayX: 'center', overlayY: 'top', offsetY: 8 },
            { originX: 'end', originY: 'top', overlayX: 'end', overlayY: 'bottom', offsetY: -8 },
      ];

      toggleMenu(i: number, ev: MouseEvent) {
            ev.stopPropagation();
            this.menuOpenIndex = this.menuOpenIndex === i ? null : i;
      }

      closeMenu() {
            this.menuOpenIndex = null;
      }

      onActionClick(action: ActionMenuOption) {
            action.action?.();
            this.closeMenu();
      }
}