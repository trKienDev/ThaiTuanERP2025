import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { LedgerAccountTypeApiService } from "../../services/api/ledger-account-type-api.service";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { HttpErrorHandlerService } from "../../../../core/services/http-errror-handler.service";
import { firstValueFrom } from "rxjs";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { LedgerAccountTypeFacade } from "../../facades/ledger-account-type.facade";

@Component({
      selector: 'ledger-account-type-excel-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent],
      templateUrl: './ledger-account-type-excel-dialog.component.html'
})
export class LedgerAccountTypeExcelDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<LedgerAccountTypeExcelDialogComponent>);
      public submitting: boolean = false;
      private readonly ledgerAccountTypeApi = inject(LedgerAccountTypeApiService);
      private readonly toast = inject(ToastService);
      private readonly ledgerAccountTypeFacade = inject(LedgerAccountTypeFacade);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);

      // === Files ===
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
            const url = 'assets/templates/ledger-account-type-template.xlsx';
            const link = document.createElement('a');
            link.href = url;
            link.download = 'ledger-account-type-template.xlsx';
            link.click();
      }

      async submit() {
            if (!this.selectedFile) {
                  this.toast.errorRich("Vui lòng chọn file Excel");
                  return;
            }

            try {
                  this.submitting = true;
                  await firstValueFrom(this.ledgerAccountTypeApi.importExcel(this.selectedFile));
                  this.toast.successRich('Thêm loại tài khoản hạch toán thành công');
                  this.ledgerAccountTypeFacade.refresh();
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