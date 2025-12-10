import { Component } from "@angular/core";
import { KitBaseIcon } from "../directives/kit-base-icon.directive";

@Component({
      selector: 'kit-square-up-arrow',
      standalone: true,
      template: `
            <svg fill="#000000" [attr.width]="computedSize" [attr.height]="computedSize" viewBox="-2 -2 24 24" xmlns="http://www.w3.org/2000/svg" preserveAspectRatio="xMinYMin" class="jam jam-arrow-square-up-f">
                  <path d='M10.707 5.293a.997.997 0 0 0-1.414 0L5.05 9.536a1 1 0 0 0 1.414 1.414L9 8.414V14a1 1 0 0 0 2 0V8.414l2.536 2.536a1 1 0 0 0 1.414-1.414l-4.243-4.243zM4 0h12a4 4 0 0 1 4 4v12a4 4 0 0 1-4 4H4a4 4 0 0 1-4-4V4a4 4 0 0 1 4-4z' />
            </svg>
      `
})
export class KitSquareUpArrowComponent extends KitBaseIcon {}