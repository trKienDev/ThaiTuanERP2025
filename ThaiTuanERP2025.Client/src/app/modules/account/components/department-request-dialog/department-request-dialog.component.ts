import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import {  MatDialogRef } from "@angular/material/dialog";
import { KitDropdownOption, KitDropdownComponent } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { UserOptionStore } from "../../options/user-dropdown-options.store";
import { DepartmentOptionStore } from "../../options/department-dropdown-options.option";
import { DepartmentRequest } from "../../models/department.model";
import { firstValueFrom } from "rxjs";
import { DepartmentFacade } from "../../facades/department.facade";

@Component({
      selector: 'department-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent],
      templateUrl: './department-request-dialog.component.html',
})
export class DepartmentRequestDialog {
      private readonly dialogRef = inject(MatDialogRef<DepartmentRequestDialog>);
      private readonly toastService = inject(ToastService);
      private readonly formBuilder = inject(FormBuilder);
      private readonly userOptionStore = inject(UserOptionStore);
      private readonly departmentOptionStore = inject(DepartmentOptionStore);
      private  readonly departmentFacade = inject(DepartmentFacade);

      dialogTitle = 'Thêm phòng ban mới';
      submitting = false;
      managerOptions$ = this.userOptionStore.option$;

      departments$ = this.departmentFacade.departments$;
      departmentOptions$ = this.departmentOptionStore.option$;
      
      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.max(200)] }),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required] }),
            parentId: this.formBuilder.control<string | null>(null),
            managerId: this.formBuilder.control<string | null>(null),
      })

      onManagerSelected(opt: KitDropdownOption | null) {
            this.form.patchValue({ managerId: opt?.id ?? null });
      }
      onDepartmentSelected(opt: KitDropdownOption | null) {
            this.form.patchValue({ parentId: opt?.id ?? null });
      }

      async submit(): Promise<void> {
            this.form.markAllAsTouched();
            if(this.form.invalid) {
                  console.error('Error');
                  return;
            }

            this.submitting = true;

            try {
                  const payload: DepartmentRequest = this.form.getRawValue();
                  const created = await firstValueFrom(this.departmentFacade.create(payload));
                  this.toastService.successRich('Thêm phòng ban thành công');
                  this.dialogRef.close({ isSuccess: true, result: created });
            } catch(error) {
                  const messages = handleHttpError(error).join('\n');
                  this.toastService.errorRich(messages || 'Lỗi khi thêm phòng ban');
            } finally {
                  this.submitting = false;
            }
      }
 
      close(result?: any) {
            this.dialogRef.close();
      }
}