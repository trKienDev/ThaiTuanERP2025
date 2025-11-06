import { CommonModule } from "@angular/common";
import { Component, Inject } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from "@angular/material/dialog";

export type ConfirmDialogData = {
      title?: string;
      message?: string;
      confirmText?: string;
      cancelText?: string;
      tone?: 'danger' | 'warning' | 'primary' |  'error';
}

@Component({
      selector: 'kit-confirm-dialog',
      standalone: true,
      imports: [ CommonModule, MatDialogModule, MatButtonModule ],
      templateUrl: './confirm-dialog.component.html',
      styleUrl: './confirm-dialog.component.scss'
})
export class KitConfirmDialogComponent {
      constructor(
            private readonly ref: MatDialogRef<KitConfirmDialogComponent, boolean>,
            @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData
      ) {}

      get buttonColor(): 'primary' | 'warn' | 'danger' | 'error' | undefined {
            if(this.data?.tone === 'danger') return 'danger';
            if(this.data?.tone === 'warning') return 'warn';
            if(this.data.tone === 'error') return 'error';

            return 'primary';
      }

      close(result: boolean) {
            this.ref.close(result);
      }
}