import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { HttpErrorHandlerService } from "../../../../core/services/http-errror-handler.service";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { CashoutGroupOptionStore } from "../../options/cashout-group.option";
import { KitDropdownComponent } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { firstValueFrom, flatMap } from "rxjs";
import { CashoutGroupPayload } from "../../models/cashout-group.model";
import { CashoutGroupFacade } from "../../facades/cashout-group.facade";

@Component({
      selector: 'cashout-group-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitSpinnerButtonComponent, KitDropdownComponent],
      templateUrl: './cashout-group-request-dialog.component.html',
})
export class CashoutGroupRequestDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<CashoutGroupRequestDialogComponent>);
      public submitting: boolean = false;
      public showErrors: boolean = false;
      private readonly toast = inject(ToastService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly formBuilder = inject(FormBuilder);
      public cashoutGroupOptions$ = inject(CashoutGroupOptionStore).options$;
      private readonly cashoutGroupFacade = inject(CashoutGroupFacade);

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            parentId: this.formBuilder.control<string | null>(null),
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

                  const payload: CashoutGroupPayload = this.form.getRawValue();
                  await firstValueFrom(this.cashoutGroupFacade.create(payload));
                  this.toast.successRich("Tạo nhóm khoản tiền ra thành công");
                  this.showErrors = false;
                  this.form.reset();
                  this.dialogRef.close(true);
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Tạo nhóm khoản tiền ra thất bại");
            } finally {
                  this.submitting = false;
                  this.form.enable({ emitEvent: true });
            }
      }

      close(isSuccess: boolean = false) {
            this.dialogRef.close(isSuccess);
      }
}