import { LedgerAccountApiService } from './../../services/api/ledger-account-api.service';
import { LedgerAccountOptionStore } from './../../options/ledger-account.option';
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { HttpErrorHandlerService } from "../../../../core/services/http-errror-handler.service";
import { CashoutCodePayload } from "../../models/cashout-code.model";
import { firstValueFrom } from "rxjs";
import { CashoutCodeFacade } from "../../facades/cashout-code.facade";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { CashoutGroupOptionStore } from "../../options/cashout-group.option";
import { KitDropdownComponent } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { TextareaNoSpellcheckDirective } from "../../../../shared/directives/textarea/textarea-no-spellcheck.directive";

@Component({
      selector: 'cashout-code-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitSpinnerButtonComponent, KitDropdownComponent, TextareaNoSpellcheckDirective],
      templateUrl: './cashout-code-request-dialog.component.html'
})
export class CashoutCodeRequestDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<CashoutCodeRequestDialogComponent>);
      public submitting: boolean = false;
      public showErrors: boolean = false;
      private readonly toast = inject(ToastService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly formBuilder = inject(FormBuilder);
      private readonly cashoutCodeFacade = inject(CashoutCodeFacade);
      public cashoutGroupOptions$ = inject(CashoutGroupOptionStore).options$;
      public ledgerAccountsOptions$ = inject(LedgerAccountOptionStore).options$;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            groupId: this.formBuilder.control<string | null>(null, { nonNullable: true, validators: [Validators.required] }),
            ledgerAccountId: this.formBuilder.control<string | null>(null, { nonNullable: true, validators: [Validators.required] }),
            description: this.formBuilder.control<string | null>(null),
      })
      

      async submit() {
            this.showErrors = true;
            
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đẩy đủ thông tin");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const raw = this.form.getRawValue();
                  const payload: CashoutCodePayload = {
                        ...raw,
                        groupId: raw.groupId as string,
                        ledgerAccountId: raw.ledgerAccountId as string
                  }
                  await firstValueFrom(this.cashoutCodeFacade.create(payload));
                  this.toast.successRich("Tạo khoản chi thành công");
                  this.showErrors = false;
                  this.form.reset();
                  this.dialogRef.close(true);
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Tạo khoản chi thất bại");
            } finally {
                  this.submitting = false;
                  this.form.enable({ emitEvent: true });
            }
      }
      
      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }
}