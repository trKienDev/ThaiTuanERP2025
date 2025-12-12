import { Component, inject, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";

@Component({
      selector: 'pdf-attachment-preview-dialog',
      standalone: true,
      template: `
            <div class="preview-container">
                  <div class="display-flex justify-content-end">
                        <button type="button" class="exit-button" (click)="close()">
                              <span class="text material-icons-outlined">close</span>
                        </button>
                  </div>
                  <div class="pdf-wrapper">
                        <embed [src]="safeUrl" type="application/pdf" class="pdf-viewer" />
                  </div>
            </div>
      `,
      styles: [`
            .preview-container {
                  border-radius: 10px;
                  box-shadow: rgba(255, 255, 255, 0.77) 0px 10px 36px 0px, rgba(255, 255, 255, 0.09) 0px 0px 0px 1px;
                  background: white;
                  width: 80vw;
                  height: 90vh;
                  display: flex;
                  flex-direction: column;
            }
            
            .pdf-wrapper {
                  padding: 10px;
                  height: 100%;
            }
            .pdf-viewer {
                  width: 100%;
                  height: 100%;
                  border: none;
            }
            .exit-button {
                  right: 0;
                  z-index: 10;
                  width: 32px;
                  height: 32px;
                  display: flex;
                  justify-content: center;
                  align-items: center;
                  cursor: pointer;
                  box-shadow: 0 2px 6px rgba(0,0,0,0.2);
                  background: white;
                  border-radius: 2px;
                  padding: 0 22px;
            }
            .exit-button span {
                  color: red; 
                  font-size: 20px;
            }
            .exit-button:hover {
                  background: red;
            }
            .exit-button:hover span {
                  color: white;
            }
      `]
})
export class PdfAttachmentPreviewDialog {
      private readonly dialogRef = inject(MatDialogRef<PdfAttachmentPreviewDialog>);


      safeUrl: SafeResourceUrl;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: { src: string },
            private sanitizer: DomSanitizer
      ) {
            this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(data.src);
      }

      close() {
            this.dialogRef.close();
      }
}
