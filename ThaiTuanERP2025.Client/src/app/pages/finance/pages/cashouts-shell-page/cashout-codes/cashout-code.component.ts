import { CommonModule } from "@angular/common";
import { ChangeDetectionStrategy, Component } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CashoutCodeRequestDialogComponent } from "./cashout-code-request-dialog/cashout-code-request-dialog.component";

@Component({
      selector: 'cashout-code-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './cashout-code.component.html',
      changeDetection: ChangeDetectionStrategy.OnPush
})
export class CashoutCodePanelComponent {
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