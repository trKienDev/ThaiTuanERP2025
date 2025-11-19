import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { LedgerAccountTypeRequestDialogComponent } from "../../../components/ledger-account-type-request-dialog/ledger-account-type-request-dialog.component";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";

@Component({
      selector: 'ledger-account-type-panel',
      standalone: true,
      imports: [CommonModule, HasPermissionDirective],
      templateUrl: './ledger-account-type-panel.component.html'
})
export class LedgerAccountTypePanelComponent {
      private readonly dialog = inject(MatDialog);

      openLedgerAccountTypeRequestDialog() {
            const dialogRef = this.dialog.open(LedgerAccountTypeRequestDialogComponent);
      }
}