import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { CashoutCodeRequestDialogComponent } from "../../../components/cashout-code-request-dialog/cashout-code-request-dialog.component";


@Component({
      selector: 'cashout-code-panel',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './cashout-code-panel.component.html',
})
export class CashoutCodePanelComponent {
      private readonly dialog = inject(MatDialog);

      openCashoutCodeRequestDialog(): void {
            const dialogRef = this.dialog.open(CashoutCodeRequestDialogComponent);
      }
}