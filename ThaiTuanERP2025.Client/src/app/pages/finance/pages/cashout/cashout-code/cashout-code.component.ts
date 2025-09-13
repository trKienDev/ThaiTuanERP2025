import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CashoutCodeRequestDialogComponent } from "../../cashouts-shell-page/cashout-codes/cashout-code-request-dialog/cashout-code-request-dialog.component";

@Component({
      selector: 'finance-cashout-code',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './cashout-code.component.html',
      styleUrl: './cashout-code.component.scss',
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class CashoutCodeComponent {
      constructor(private dialog: MatDialog) {}

      openCreateCashoutCodeModal(): void {
            const dialogRef = this.dialog.open(CashoutCodeRequestDialogComponent, {
                  width: '520px',
                  disableClose: true
            });

            dialogRef.afterClosed().subscribe((result) => {
                  if(result === 'created') {
                        close();
                  }
            })
      }
}