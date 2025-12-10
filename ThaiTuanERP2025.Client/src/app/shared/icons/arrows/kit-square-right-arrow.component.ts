import { Component } from "@angular/core";
import { KitBaseIcon } from "../directives/kit-base-icon.directive";

@Component({
      selector: 'kit-square-right-arrow',
      standalone: true,
      template: `
            <svg fill="#000000" [attr.width]="computedSize" [attr.height]="computedSize"  viewBox="-2 -2 24 24" xmlns="http://www.w3.org/2000/svg" preserveAspectRatio="xMinYMin" class="jam jam-arrow-square-right-f">
                  <path d='M14.707 10.707a.997.997 0 0 0 0-1.414L10.464 5.05A1 1 0 0 0 9.05 6.464L11.586 9H6a1 1 0 1 0 0 2h5.586L9.05 13.536a1 1 0 0 0 1.414 1.414l4.243-4.243zM4 0h12a4 4 0 0 1 4 4v12a4 4 0 0 1-4 4H4a4 4 0 0 1-4-4V4a4 4 0 0 1 4-4z' />
            </svg>
      `
})
export class KitSquareRightArrowComponent extends KitBaseIcon {}