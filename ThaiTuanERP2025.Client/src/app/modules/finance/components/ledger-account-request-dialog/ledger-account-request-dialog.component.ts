import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { LedgerAccountBalanceType } from '../../models/ledger-account.model';
import { KitDropdownComponent, KitDropdownOption } from '../../../../shared/components/kit-dropdown/kit-dropdown.component';
import { LedgerAccountTypeFacade } from '../../facades/ledger-account-type.facade';
import { LedgerAccountTypeOptionStore } from '../../options/ledger-account-type.option';
import { LedgerAccountOptionStore } from '../../options/ledger-account.option';
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'ledger-account-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent, KitSpinnerButtonComponent],
      templateUrl: './ledger-account-request-dialog.component.html'
})
export class LedgerAccountRequestDialogComponent {
      private readonly dialog = inject(MatDialogRef<LedgerAccountRequestDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);

      public submitting: boolean = false;
      public showErrors: boolean = false;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            number: this.formBuilder.control<string>('' ,{ nonNullable: true, validators: [Validators.required] }),
            balanceType: this.formBuilder.control<LedgerAccountBalanceType>(0, { nonNullable: true, validators: [ Validators.required ]}),
            ledgerAccountTypeId: this.formBuilder.control<string>('' ,{ nonNullable: true, validators: [Validators.required] }),
            description: this.formBuilder.control<string | null>(null),
            parentLedgerAccountId: this.formBuilder.control<string | null>(null),
      })

      // LedgerAccountBalanceType
      balanceTypeOptions: KitDropdownOption[] = [
            { id: 'none', label: 'Không có số dư' },
            { id: 'debit', label: 'Dư nợ' },
            { id: 'credit', label: 'Dư có' },
            { id: 'both', label: 'Lưỡng tính' },
      ];
      onBalanceTypeSelected(opt: KitDropdownOption) {
            switch(opt.id) {
                  case 'debit': 
                        this.form.patchValue({ balanceType: 1 });
                        break;
                  case 'credit':
                        this.form.patchValue({ balanceType: 2 })
                        break;
                  case 'both':
                        this.form.patchValue({ balanceType: 3 })
                        break;
                  default:
                        this.form.patchValue({ balanceType: 0 });
                        break;
            }
      }

      // ledger account type
      public ledgerAccountTypeOptions$ = inject(LedgerAccountTypeOptionStore).options$;
      onLedgerAccountTypeSeleted(opt: KitDropdownOption) {
            this.form.patchValue({ ledgerAccountTypeId: opt.id });
      }

      // ledger account options
      public parentLedgerAccountOptions$ = inject(LedgerAccountOptionStore).options$;
      onParentLedgerAccountSelected(opt: KitDropdownOption) {
            this.form.patchValue({ parentLedgerAccountId: opt.id })
      }

      close(isSuccess: boolean = false) {
            this.dialog.close(isSuccess);
      }
}