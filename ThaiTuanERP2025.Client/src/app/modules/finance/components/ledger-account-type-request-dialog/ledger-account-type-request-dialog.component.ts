import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { LedgerAccountTypeKind, LedgerAccountTypePayload } from "../../models/ledger-account-type.model";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { HttpErrorHandlerService } from "../../../../core/services/http-errror-handler.service";
import { firstValueFrom } from "rxjs";
import { LedgerAccountTypeFacade } from "../../facades/ledger-account-type.facade";

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
      private readonly toast = inject(ToastService);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly formBuilder = inject(FormBuilder);
      private readonly LAccountTypeFacade = inject(LedgerAccountTypeFacade);

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            kind: this.formBuilder.control<LedgerAccountTypeKind | null>(null, { nonNullable: true, validators: [ Validators.required ] }),
            description: this.formBuilder.control<string | null>(null),
      })

      // LedgerAccountTypeKind
      typeKindOptions: KitDropdownOption[] = [
            { id: LedgerAccountTypeKind.none, label: 'Không có' },
            { id: LedgerAccountTypeKind.asset, label: 'Tài sản' },
            { id: LedgerAccountTypeKind.liability, label: 'Nợ' },
            { id: LedgerAccountTypeKind.equity, label: 'Vốn chủ sở hữu' },
            { id: LedgerAccountTypeKind.revenue, label: 'Doanh thu' },
            { id: LedgerAccountTypeKind.expense, label: 'Chi phí' },
      ];

      // submit
      async submit(): Promise<void> {
            this.showErrors = true;
            
            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đẩy đủ thông bắt buộc");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const raw = this.form.getRawValue();
                  const payload: LedgerAccountTypePayload = {
                        ...raw,
                        kind: raw.kind as LedgerAccountTypeKind
                  }
                  await firstValueFrom(this.LAccountTypeFacade.create(payload));
                  this.toast.successRich("Tạo loại tài khoản hạch toán thành công");
                  this.showErrors = false;
                  this.form.reset();
                  this.dialog.close(true);
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Tạo loại tài khoản thất bại");
            } finally {
                  this.submitting = false;
                  this.form.enable({ emitEvent: true });
            }
      }

      close(isSuccess: boolean = false) {
            this.dialog.close(isSuccess);
      }
}