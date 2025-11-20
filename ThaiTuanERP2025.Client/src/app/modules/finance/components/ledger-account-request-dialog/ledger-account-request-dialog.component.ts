import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { LedgerAccountBalanceType, LedgerAccountPayload } from '../../models/ledger-account.model';
import { KitDropdownComponent, KitDropdownOption } from '../../../../shared/components/kit-dropdown/kit-dropdown.component';
import { LedgerAccountTypeOptionStore } from '../../options/ledger-account-type.option';
import { LedgerAccountOptionStore } from '../../options/ledger-account.option';
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { ToastService } from '../../../../shared/components/kit-toast-alert/kit-toast-alert.service';
import { HttpErrorHandlerService } from '../../../../core/services/http-errror-handler.service';
import { firstValueFrom } from 'rxjs';
import { LedgerAccountFacade } from '../../facades/ledger-account.facade';

@Component({
      selector: 'ledger-account-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent, KitSpinnerButtonComponent],
      templateUrl: './ledger-account-request-dialog.component.html'
})
export class LedgerAccountRequestDialogComponent {
      private readonly dialog = inject(MatDialogRef<LedgerAccountRequestDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toast = inject(ToastService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly ledgerAccountFacade = inject(LedgerAccountFacade);

      public submitting: boolean = false;
      public showErrors: boolean = false;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            number: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            balanceType: this.formBuilder.control<LedgerAccountBalanceType | null>(null, { nonNullable: true, validators: [Validators.required  ] }),
            ledgerAccountTypeId: this.formBuilder.control<string | null>(null),
            description: this.formBuilder.control<string | null>(null),
            parentLedgerAccountId: this.formBuilder.control<string | null>(null),
      })

      // LedgerAccountBalanceType
      balanceTypeOptions: KitDropdownOption[] = [
            { id: LedgerAccountBalanceType.none, label: 'Không có số dư' },
            { id: LedgerAccountBalanceType.debit, label: 'Dư nợ' },
            { id: LedgerAccountBalanceType.credit, label: 'Dư có' },
            { id: LedgerAccountBalanceType.both, label: 'Lưỡng tính' },
      ];

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

      async submit() {
            this.showErrors = true;
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đầy đủ thông tin bắt buộc");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const raw = this.form.getRawValue();
                  const payload: LedgerAccountPayload = {
                        ...raw,
                        balanceType: raw.balanceType as LedgerAccountBalanceType
                  };

                  console.log('payload: ', payload);
                  await firstValueFrom(this.ledgerAccountFacade.create(payload));
                  this.toast.successRich("Tạo tài khoản hạch toán thành công");
                  this.showErrors = false;
                  this.form.reset();
                  this.dialog.close();
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Tạo tài khoản hạch toán thất bại");
            } finally {
                  this.submitting = false;
                  this.form.enable({ emitEvent: true });
            }
      }

      close(isSuccess: boolean = false) {
            this.dialog.close(isSuccess);
      }
}