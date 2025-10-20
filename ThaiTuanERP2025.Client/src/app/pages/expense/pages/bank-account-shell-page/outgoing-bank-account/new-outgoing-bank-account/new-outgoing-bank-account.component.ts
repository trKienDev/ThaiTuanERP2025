import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { OutgoingBankAccountFacade } from "../../../../facades/outgoing-bank-account.facade";
import { OutgoingBankAccountRequest } from "../../../../models/outgoing-bank-account.model";
import { catchError, of } from "rxjs";
import { handleHttpError } from "../../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../../shared/components/toast/toast.service";

@Component({
      selector: 'new-outgoing-bank-account-dialog',
      standalone: true,
      templateUrl: './new-outgoing-bank-account.component.html',
      imports: [ CommonModule, ReactiveFormsModule ]
})
export class NewOutgoingBankAccountDialogComponent {
      private dialogRef = inject(MatDialogRef<NewOutgoingBankAccountDialogComponent>);
      private formBuilder = inject(FormBuilder); 
      private OBAFacade = inject(OutgoingBankAccountFacade);
      private toast = inject(ToastService);

      loading = false;
      submitting = false;
      errorMessages: string[] = [];

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
            bankName: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
            accountNumber: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
            ownerName: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] }),
      });

      async submit(): Promise<void> {
            if (this.form.invalid) {
                  this.form.markAllAsTouched();
                  return;
            }

            this.submitting = true;

            try {
                  const payload = this.form.getRawValue() as OutgoingBankAccountRequest;
                  console.log('Payload to submit:', payload);
                  this.OBAFacade.create(payload).pipe(
                        catchError(err => {
                              this.errorMessages = handleHttpError(err);
                              this.submitting = false;
                              return of(null);
                        })
                  ).subscribe((created) => {
                        this.toast.successRich("Tạo tài khoản tiền ra thành công");
                        this.dialogRef.close(true);
                  });
            } catch (error) {
                  this.toast.errorRich("Có lỗi xảy ra, vui lòng thử lại");
                  console.error(error);
            } finally {
                  this.submitting = false;
            }
      }

      close(): void {
            this.dialogRef.close();
      }
}