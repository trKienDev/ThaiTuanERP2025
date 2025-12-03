import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { HttpErrorHandlerService } from "../../../../core/services/http-errror-handler.service";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { SupplierFacade } from "../../facades/supplier.facade";
import { firstValueFrom } from "rxjs";

@Component({
      selector: 'supplier-request-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent, ReactiveFormsModule],
      templateUrl: './supplier-request-dialog.component.html' 
})
export class SupplierRequestDialogComponent {
      private readonly dialogRef = inject(MatDialogRef<SupplierRequestDialogComponent>);
      private readonly formBuilder = inject(FormBuilder);
      private readonly httpErrorHandler = inject(HttpErrorHandlerService);
      private readonly toast = inject(ToastService);
      private readonly supplierFacade = inject(SupplierFacade);

      public showErrors: boolean = false;
      public submitting: boolean = false;

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>("", { nonNullable: true, validators: [ Validators.required ]}),
            taxCode: this.formBuilder.control<string | null>(null)
      });

      async submit(): Promise<void> {
            alert('submit');
            this.showErrors = true;

            if(this.form.invalid) {
                  this.form.markAllAsTouched();
                  this.toast.warningRich("Vui lòng điền đẩy đủ thông tin");
                  return;
            }

            try {
                  this.submitting = true;
                  this.form.disable({ emitEvent: false });

                  const payload = this.form.getRawValue();
                  const created = await firstValueFrom(this.supplierFacade.create(payload));

                  this.toast.successRich("Thêm nhà cung cấp mới thành công");
                  this.showErrors = false;
                  this.close(created);
            } catch(error) {
                  this.httpErrorHandler.handle(error, "Thêm nhà cung cấp mới thất bại");
            } finally {
                  this.submitting = false;
                  this.form.enable({ emitEvent: false });
            }
      }     

      close(createdSupplierId?: string) {
            this.dialogRef.close(createdSupplierId);
      }
}