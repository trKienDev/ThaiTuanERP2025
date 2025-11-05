import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CashoutCodeRequestDialogComponent } from "./cashout-code-request-dialog/cashout-code-request-dialog.component";
import { CashoutCodeDto } from "../../../models/cashout-code.model";
import { CashoutCodeService } from "../../../services/cashout-code.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";

@Component({
      selector: 'cashout-code-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './cashout-code.component.html',
})
export class CashoutCodePanelComponent implements OnInit {
      private cashoutCodeService = inject(CashoutCodeService);
      private toastService = inject(ToastService);
      
      cashoutCodes: CashoutCodeDto[] = [];

      constructor(private dialog: MatDialog) {}

      ngOnInit(): void {
            this.loadCashoutCodes();
      }

      loadCashoutCodes(): void {
            this.cashoutCodeService.getAll().subscribe({
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