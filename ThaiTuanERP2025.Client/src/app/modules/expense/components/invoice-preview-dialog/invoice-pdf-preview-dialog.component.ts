import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";

@Component({
      selector: 'invoice-pdf-preview-dialog',
      standalone: true,
      template: `
            <div class="pdf-wrapper">
                  <embed [src]="data.src" type="application/pdf" class="pdf-viewer" />
            </div>
      `,
      styles: [`
            .pdf-wrapper {
                  width: 80vw;
                  height: 90vh;
            }
            .pdf-viewer {
                  width: 100%;
                  height: 100%;
                  border: none;
            }
      `]
})
export class InvoicePdfPreviewDialog {
      constructor(@Inject(MAT_DIALOG_DATA) public data: { src: string }) {}
}
