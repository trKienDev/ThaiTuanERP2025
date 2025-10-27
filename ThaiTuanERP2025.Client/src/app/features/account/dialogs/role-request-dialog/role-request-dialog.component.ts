import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RoleRequest } from "../../models/role.model";
import { firstValueFrom } from "rxjs";
import { RoleService } from "../../services/role.service";
import { RoleFacade } from "../../facades/role.facade";

@Component({
      selector: "role-request-dialog",
      templateUrl: "./role-request-dialog.component.html",
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule ],
})
export class RoleRequestDialogComponent {
      private matDialogRef = inject(MatDialogRef<RoleRequestDialogComponent>);
      private toast = inject(ToastService);
      private formBuilder = inject(FormBuilder);
      private roleFacade = inject(RoleFacade);

      public submitting = false;    

      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [Validators.required] }),
            description: this.formBuilder.control<string>('', { nonNullable: false }),
      });

      async submit(): Promise<void> {
            if(this.form.invalid) {
                  this.form.markAllAsTouched();

                  const invalidControls = Object.entries(this.form.controls)
                        .filter(([_, control]) => control.invalid)
                        .map(([name, control]) => ({
                              field: name,
                              errors: control.errors
                        }));

                  console.group('⚠️ Form invalid');
                  console.table(invalidControls);
                  console.groupEnd();

                  // Scroll đến control đầu tiên bị lỗi
                  const firstInvalidControl = document.querySelector('.ng-invalid[formControlName]') as HTMLElement;
                  if (firstInvalidControl) {
                        firstInvalidControl.scrollIntoView({ behavior: 'smooth', block: 'center' });
                        firstInvalidControl.focus();
                  }

                  this.toast.warningRich("Vui lòng điền tên vai trò");
                  return;
            }

            this.submitting = true;
            try {
                  const payload = this.form.getRawValue() as RoleRequest;
                  console.log('Submitting role request:', payload);
                  await firstValueFrom(this.roleFacade.create(payload));
                  this.toast.successRich("Tạo role mới thành công");
                  this.roleFacade.refresh();
                  this.closeDialog({ success: true });
                  return;
            } catch(error) {  
                  this.toast.errorRich("Đã có lỗi xảy ra khi gửi yêu cầu vai trò");
                  console.error(error);
            } finally {
                  this.submitting = false;
            }
      }

      closeDialog(result?: { success?: boolean; }) {
            this.matDialogRef.close(result);
      }
}