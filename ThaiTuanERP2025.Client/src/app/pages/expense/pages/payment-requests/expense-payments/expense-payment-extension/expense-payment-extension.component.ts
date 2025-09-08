import { Component, Inject } from "@angular/core";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../../shared/components/kit-dropdown/kit-dropdown.component"
import { CommonModule } from "@angular/common";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";
import { FormsModule } from "@angular/forms";

export type ExpensePaymentExtensionData = {
      budgetCodeOptions: KitDropdownOption[];
      cashoutCodeOptions: KitDropdownOption[];
      budgetCodeId?: string | null;
      cashoutCodeId?: string | null;
}

@Component({
      selector: 'expense-payment-extensions',
      standalone: true,
      imports: [ CommonModule, MatDialogModule, MatButtonModule, FormsModule, KitDropdownComponent ],    
      templateUrl: './expense-payment-extension.component.html',
})
export class ExpensePaymentExtensionComponent {
      budgetCodeId: string | null;
      cashoutCodeId: string | null;

      constructor(
            private ref: MatDialogRef<ExpensePaymentExtensionComponent>,
            @Inject(MAT_DIALOG_DATA) public data: ExpensePaymentExtensionData
      ) {
            this.budgetCodeId = data.budgetCodeId ?? null;
            this.cashoutCodeId = data.cashoutCodeId ?? null;
      }

      close() { this.ref.close(); }
      save() {
            this.ref.close({
                  budgetCodeId: this.budgetCodeId ?? null,
                  cashoutCodeId: this.cashoutCodeId ?? null,
            })
      }
}