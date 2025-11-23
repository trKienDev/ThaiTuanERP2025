import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { LedgerAccountTypeRequestDialogComponent } from "../../../components/ledger-account-type-request-dialog/ledger-account-type-request-dialog.component";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";
import { LedgerAccountTypeFacade } from "../../../facades/ledger-account-type.facade";
import { LedgerAccountTypeDto } from "../../../models/ledger-account-type.model";
import { LedgerAccountTypeKind } from "../../../pipes/ledger-account-type.pipe";
import { KitMathIconComponent } from "../../../../../shared/icons/kit-math-icon.component";
import { LedgerAccountTypeExcelDialogComponent } from "../../../components/ledger-account-type-excel-dialog/ledger-account-type-excel-dialog.component";
import { KitExcelIconComponent } from "../../../../../shared/icons/kit-excel-icon.component";

@Component({
      selector: 'ledger-account-type-panel',
      standalone: true,
      imports: [CommonModule, HasPermissionDirective, LedgerAccountTypeKind, KitMathIconComponent, KitExcelIconComponent],
      templateUrl: './ledger-account-type-panel.component.html'
})
export class LedgerAccountTypePanelComponent {
      private readonly dialog = inject(MatDialog);
      private readonly ledgerAccountTypeFacade = inject(LedgerAccountTypeFacade);
      public ledgerAccountType$ = this.ledgerAccountTypeFacade.ledgerAccountTypes$;
      
      trackById(index: number, item: LedgerAccountTypeDto) { return item.id; }

      openLedgerAccountTypeRequestDialog() {
            this.dialog.open(LedgerAccountTypeRequestDialogComponent);
      }

      openLedgerAccountTypeExcelDialog() {
            const dialogRef = this.dialog.open(LedgerAccountTypeExcelDialogComponent);
            dialogRef.afterClosed().subscribe((result: { success?: boolean;} | undefined ) => {
                  if(result && result.success) {
                       
                  }
            });
      }
}