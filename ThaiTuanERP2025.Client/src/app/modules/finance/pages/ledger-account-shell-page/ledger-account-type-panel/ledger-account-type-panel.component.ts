import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { LedgerAccountTypeRequestDialogComponent } from "../../../components/ledger-account-type-request-dialog/ledger-account-type-request-dialog.component";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";
import { LedgerAccountTypeFacade } from "../../../facades/ledger-account-type.facade";
import { LedgerAccountTypeDto } from "../../../models/ledger-account-type.model";
import { LedgerAccountTypeKind } from "../../../pipes/ledger-account-type.pipe";
import { KitMathIconComponent } from "../../../../../shared/icons/kit-math-icon.component";

@Component({
      selector: 'ledger-account-type-panel',
      standalone: true,
      imports: [CommonModule, HasPermissionDirective, LedgerAccountTypeKind, KitMathIconComponent],
      templateUrl: './ledger-account-type-panel.component.html'
})
export class LedgerAccountTypePanelComponent {
      private readonly dialog = inject(MatDialog);
      public ledgerAccountType$ = inject(LedgerAccountTypeFacade).ledgerAccountTypes$;

      trackById(index: number, item: LedgerAccountTypeDto) { return item.id; }

      openLedgerAccountTypeRequestDialog() {
            this.dialog.open(LedgerAccountTypeRequestDialogComponent);
      }
}