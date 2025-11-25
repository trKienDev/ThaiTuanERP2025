import { CommonModule } from "@angular/common";
import { Component, inject, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";

@Component({
      selector: 'invoice-image-preview-dialog',
      standalone: true,
      imports: [ CommonModule ],
      template: `
            <div class="preview-container">
                  <button type="button" class="exit-button" (click)="close()">
                        <span class="text material-icons-outlined">close</span>
                  </button>
                  <div class="img-wrapper">
                        <div class="scroll">
                              <img [src]="data.src" class="preview-img"/>
                        </div>
                  </div>
            </div>
      `,
      styles: [`
            .preview-container {
                  border-radius: 10px;
                  box-shadow: rgba(255, 255, 255, 0.77) 0px 10px 36px 0px, rgba(255, 255, 255, 0.09) 0px 0px 0px 1px;
                  background: white;
                  position: relative;
            }
            .img-wrapper {
                  padding: 10px;
            }
            .scroll {
                  overflow: auto;
                  max-width: 80vw;
                  max-height: 80vh;
            }
            .preview-img {
                  display: block;
                  max-width: 100%;
                  height: auto;     /* Quan trọng */
                  object-fit: unset;
            }
            .exit-button {
                  position: absolute;      /* đè lên ảnh */
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
            }
            .exit-button span {
                  color: red; 
            }
            .exit-button:hover {
                  background: red;
            }
            .exit-button:hover span {
                  color: white;
            }
      `]
})
export class InvoiceImagePreviewDialog {
      private readonly dialogRef = inject(MatDialogRef<InvoiceImagePreviewDialog>);

      constructor(@Inject(MAT_DIALOG_DATA) public data: { src: string }) {}

      close() {
            this.dialogRef.close();
      }
}