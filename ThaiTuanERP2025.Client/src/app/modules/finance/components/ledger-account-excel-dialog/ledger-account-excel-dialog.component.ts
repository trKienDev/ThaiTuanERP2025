import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { HttpErrorHandlerService } from "../../../../core/services/http-errror-handler.service";
import { LedgerAccountApiService } from "../../services/api/ledger-account-api.service";
import { firstValueFrom } from "rxjs";

@Component({
      selector: 'ledger-account-excel-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent],
      templateUrl: './ledger-account-excel-dialog.component.html'
})
export class LedgerAccountExcelDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<LedgerAccountExcelDialogComponent>);
      public submitting: boolean = false;
      private readonly toast = inject(ToastService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly ledgerAccountApi = inject(LedgerAccountApiService);

      // ==== FILES ====
      selectedFile: File | null = null;
      onFileChosen(event: any) {
            const file: File = event.target.files[0];
            if (!file) return;

            this.selectedFile = file;
      }
      removeFile() {
            this.selectedFile = null;

            // reset input file để user có thể chọn lại cùng một file
            const input = document.getElementById('uploader-input') as HTMLInputElement;
            if (input) input.value = '';
      }
      downloadTemplate() {
            const url = 'assets/templates/ledger-account-template.xlsx';
            const link = document.createElement('a');
            link.href = url;
            link.download = 'ledger-account-template.xlsx';
            link.click();
      }

      async submit() {
            if (!this.selectedFile) {
                  this.toast.errorRich("Vui lòng chọn file Excel");
                  return;
            }

            try {
                  this.submitting = true;
                  await firstValueFrom(this.ledgerAccountApi.importExcel(this.selectedFile));
                  this.toast.successRich('Thêm loại tài khoản hạch toán thành công');
                  this.close(true);
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Thêm loại tài khoản hạch toán thất bại");
            } finally {
                  this.submitting = false;
            }
      }

      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }
}