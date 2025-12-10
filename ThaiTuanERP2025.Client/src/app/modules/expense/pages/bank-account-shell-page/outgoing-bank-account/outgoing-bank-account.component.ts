import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { OutgoingBankAccountFacade } from "../../../facades/outgoing-bank-account.facade";
import { OutgoingBankAccountDto } from "../../../models/outgoing-bank-account.model";
import { OutgoingBankAccountRequestDialogComponent } from "../../../components/dialogs/outgoing-bank-account-request-dialog/outgoing-bank-account-request-dialog.component";
import { HasPermissionDirective } from "../../../../../core/auth/auth.directive";

@Component({
      selector: 'outgoing-bank-account',
      standalone: true,
      templateUrl: './outgoing-bank-account.component.html',
      imports: [CommonModule, HasPermissionDirective],
})
export class OutgoingBankAccountComponent {
      private dialog = inject(MatDialog);
      public submitting = false;

      private outgoingBankFacade = inject(OutgoingBankAccountFacade);
      outgoingBankAccounts$ = this.outgoingBankFacade.outgoingBankAccounts$;
      
      trackById(index: number, item: OutgoingBankAccountDto) { return item.id; }

      openNewOutgoingBankAccountDialog(): void {
            this.dialog.open(OutgoingBankAccountRequestDialogComponent);
      }
}