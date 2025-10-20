import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { NewOutgoingBankAccountDialogComponent } from "./new-outgoing-bank-account/new-outgoing-bank-account.component";
import { OutgoingBankAccountFacade } from "../../../facades/outgoing-bank-account.facade";
import { OutgoingBankAccountDto } from "../../../models/outgoing-bank-account.model";

@Component({
      selector: 'outgoing-bank-account',
      standalone: true,
      templateUrl: './outgoing-bank-account.component.html',
      imports: [ CommonModule ],
})
export class OutgoingBankAccountComponent {
      private dialog = inject(MatDialog);
      public submitting = false;

      private OBAFacade = inject(OutgoingBankAccountFacade);
      OBAccounts$ = this.OBAFacade.outgoingBankAccounts$;
      
      trackById(index: number, item: OutgoingBankAccountDto) { return item.id; }

      openNewOutgoingBankAccountDialog(): void {
            const dialogRef = this.dialog.open(NewOutgoingBankAccountDialogComponent, {});
            dialogRef.afterClosed().subscribe(result => {
                  console.log('The dialog was closed');
            });
      }
}