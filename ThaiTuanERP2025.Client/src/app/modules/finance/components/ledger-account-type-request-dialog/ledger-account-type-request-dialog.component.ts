import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
      selector: 'ledger-account-type-request-dialog',
      standalone: true,
      imports: [ CommonModule ],
      templateUrl: './ledger-account-type-request-dialog.component.html'
})
export class LedgerAccountTypeRequestDialogComponent {
      private readonly dialog = inject(MatDialogRef<LedgerAccountTypeRequestDialogComponent>);


      close(isSuccess: boolean = false) {
            this.dialog.close(isSuccess);
      }
}