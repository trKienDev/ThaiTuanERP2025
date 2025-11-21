import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CashoutCodeDto } from "../../../models/cashout-code.model";
import { CashoutCodeApiService } from "../../../services/api/cashout-code-api.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { CashoutCodeRequestDialogComponent } from "../../../components/cashout-code-request-dialog/cashout-code-request-dialog.component";

@Component({
      selector: 'cashout-code-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './cashout-code-panel.component.html',
})
export class CashoutCodePanelComponent implements OnInit {
      private cashoutCodeApi = inject(CashoutCodeApiService);
      private toastService = inject(ToastService);
      
      cashoutCodes: CashoutCodeDto[] = [];

      constructor(private dialog: MatDialog) {}

      ngOnInit(): void {
            this.loadCashoutCodes();
      }

      loadCashoutCodes(): void {
            this.cashoutCodeApi.getAll().subscribe({
                  next: (cashoutCodes) => {
                        this.cashoutCodes = cashoutCodes;
                  },
                  error: (err => {
                        const msg = handleHttpError(err);
                        const message = Array.isArray(msg) ? msg.join('\n') : String(msg);
                        this.toastService.errorRich('Lỗi lấy mã dòng tiền ra');
                  })
            })
      }

      openCreateCashoutCodeModal(): void {
            const dialogRef = this.dialog.open(CashoutCodeRequestDialogComponent, {
                  width: '520px',
                  disableClose: true
            });

            dialogRef.afterClosed().subscribe((result) => {
                  if(result == true) 
                        this.loadCashoutCodes();
            })
      }
}