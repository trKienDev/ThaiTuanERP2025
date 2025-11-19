import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { LedgerAccountTypeKind } from "../../models/ledger-account-type.dto";

@Component({
      selector: 'ledger-account-type-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitSpinnerButtonComponent, KitDropdownComponent ],
      templateUrl: './ledger-account-type-request-dialog.component.html'
})
export class LedgerAccountTypeRequestDialogComponent {
      private readonly dialog = inject(MatDialogRef<LedgerAccountTypeRequestDialogComponent>);
      public submitting: boolean = false;
      public showErrors: boolean = false;
      private readonly formBuilder = inject(FormBuilder);

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            typeKind: this.formBuilder.control<LedgerAccountTypeKind>('asset', { nonNullable: true, validators: [ Validators.required ] }),
            description: this.formBuilder.control<string | null>(null),
      })

      // LedgerAccountTypeKind
      typeKindOptions: KitDropdownOption[] = [
            { id: 'none', label: 'Không có' },
            { id: 'asset', label: 'Tài sản' },
            { id: 'liability', label: 'Nợ' },
            { id: 'equity', label: 'Vốn chủ sở hữu' },
            { id: 'revenue', label: 'Doanh thu' },
            { id: 'expense', label: 'Chi phí' },
      ];
      onLedgerAccountTypeKindSelected(opt: KitDropdownOption) {
            switch(opt.id) {
                  case 'asset':
                        this.form.patchValue({ typeKind: 'asset' });
                        break;
                  case 'liability': 
                        this.form.patchValue({ typeKind: 'liability' });
                        break;
                  case 'equity':
                        this.form.patchValue({ typeKind: 'equity' })
                        break;
                  case 'revenue':
                        this.form.patchValue({ typeKind: 'revenue' })
                        break;
                  case 'expense':
                        this.form.patchValue({ typeKind: 'expense' });
                        break;
                  default:
                        this.form.patchValue({ typeKind: 'none' });
                        break;
            }
      }

      close(isSuccess: boolean = false) {
            this.dialog.close(isSuccess);
      }
}