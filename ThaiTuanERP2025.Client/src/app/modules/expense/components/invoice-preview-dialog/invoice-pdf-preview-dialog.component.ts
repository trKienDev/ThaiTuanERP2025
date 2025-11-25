import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";

@Component({
      selector: 'invoice-pdf-preview-dialog',
      standalone: true,
      template: `
            <div>
                  <div class="pdf-wrapper">
                        <embed [src]="safeUrl" type="application/pdf" class="pdf-viewer" />
                  </div>
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
      safeUrl: SafeResourceUrl;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: { src: string },
            private sanitizer: DomSanitizer
      ) {
            this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(data.src);
      }
}
