import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { PermissionRequest } from "../../models/permission.model";
import { firstValueFrom } from "rxjs";
import { PermissionFacade } from "../../facades/permission.facade";

@Component({
      selector: "permission-request-dialog",
      templateUrl: './permission-request-dialog.component.html',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule],
})
export class PermissionRequestDialogComponent {
      private readonly matDialogRef = inject(MatDialogRef<PermissionRequestDialogComponent>);
      private readonly toastService = inject(ToastService);    
      private readonly formBuilder = inject(FormBuilder);
      private readonly permissionFacade = inject(PermissionFacade);

      public submitting = false;

      public form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: Validators.required }),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: Validators.required }),
            description: this.formBuilder.control<string>(''),
      });

      async submit(): Promise<void> {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();

                  this.toastService.warningRich('Vui lòng điền đầy đủ thông tin bắt buộc!');
                  return;
            }

            this.submitting = true;
            try {
                  const payload = this.form.getRawValue() as PermissionRequest;
                  await firstValueFrom(this.permissionFacade.create(payload));
                  this.permissionFacade.refresh();
                  this.toastService.successRich('Tạo quyền thành công!');
                  this.closeDialog();
            } catch (error) {
                  this.toastService.errorRich('Đã có lỗi xảy ra, vui lòng thử lại!');
            } finally {
                  this.submitting = false;
            }
      }

      closeDialog(): void {
            this.matDialogRef.close();
      }
}