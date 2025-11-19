import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { KiAbacusIconComponent } from "../../../../../shared/icons/kit-abacus-icon.component";
import { MatDialog } from "@angular/material/dialog";
import { LedgerAccountRequestDialogComponent } from "../../../components/ledger-account-request-dialog/ledger-account-request-dialog.component";

@Component({
      selector: 'ledger-account-panel',
      standalone: true,
      imports: [CommonModule, KiAbacusIconComponent],
      templateUrl: './ledger-account-panel.component.html'
})
export class LedgerAccountPanelComponent {
      private readonly dialog = inject(MatDialog);

      openLedgerAccountRequestDialog() {
            this.dialog.open(LedgerAccountRequestDialogComponent);
      }
}