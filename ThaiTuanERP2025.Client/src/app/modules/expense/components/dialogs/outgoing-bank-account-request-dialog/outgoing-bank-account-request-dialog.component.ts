import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { OutgoingBankAccountFacade } from "../../../facades/outgoing-bank-account.facade";
import { firstValueFrom } from "rxjs";
import { ToastService } from "../../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { HttpErrorHandlerService } from "../../../../../core/services/http-errror-handler.service";
import { OutgoingBankAccountPayload } from "../../../models/outgoing-bank-account.model";
import { CdkAutofill } from "@angular/cdk/text-field";
import { KitSpinnerButtonComponent } from "../../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'new-outgoing-bank-account-dialog',
      standalone: true,
      templateUrl: './outgoing-bank-account-request-dialog.component.html',
      imports: [CommonModule, ReactiveFormsModule, CdkAutofill, KitSpinnerButtonComponent]
})
export class OutgoingBankAccountRequestDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<OutgoingBankAccountRequestDialogComponent>);
      private readonly formBuilder = inject(FormBuilder); 
      private readonly outgoingBankFacade = inject(OutgoingBankAccountFacade);
      private readonly toast = inject(ToastService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService); 

      public showErrors: boolean = false;
      public submitting: boolean = false;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
            ownerName: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
      });

      async submit(): Promise<void> {
            this.showErrors = true;

            if (this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đẩy đủ thông tin");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const payload = this.form.getRawValue() as OutgoingBankAccountPayload;
                  console.log('Payload to submit:', payload);
                  await firstValueFrom(this.outgoingBankFacade.create(payload));
                  this.toast.successRich("Tạo tài khoản tiền ra thành công");
                  this.showErrors = false;
                  this.dialogRef.close(true);
            } catch (error) {
                  this.httpErrorHandler.handle(error, "Lỗi khi tọa tài khoản khoản chi");
            } finally {
                  this.submitting = false;
                  this.form.enable({ emitEvent: true });
            }
      }

      close(isSuccess: boolean = false): void {
            this.dialogRef.close(isSuccess);
      }
}