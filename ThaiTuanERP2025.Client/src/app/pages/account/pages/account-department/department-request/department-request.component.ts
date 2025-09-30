import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import {  MatDialogRef } from "@angular/material/dialog";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { UserOptionStore } from "../../../options/user-dropdown-options.store";
import { DepartmentOptionStore } from "../../../options/department-dropdown-options.option";
import { DepartmentRequest } from "../../../models/department.model";
import { firstValueFrom } from "rxjs";
import { DepartmentFacade } from "../../../facades/department.facade";

@Component({
      selector: 'department-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent],
      templateUrl: './department-request.component.html',
})
export class DepartmentRequestDialog {
      private dialogRef = inject(MatDialogRef<DepartmentRequestDialog>);
      private toastService = inject(ToastService);
      private formBuilder = inject(FormBuilder);
      private userOptionStore = inject(UserOptionStore);
      private departmentOptionStore = inject(DepartmentOptionStore);
      private departmentFacade = inject(DepartmentFacade);

      dialogTitle = 'Thêm phòng ban mới';
      submitting = false;
      managerOptions$ = this.userOptionStore.option$;

      departments$ = this.departmentFacade.departments$;
      departmentOptions$ = this.departmentOptionStore.option$;

      regionOptions: KitDropdownOption[] = [
            { id: '0', label: 'Không có' },
            { id: '1', label: 'Miền Bắc' },
            { id: '2', label: 'Miền Trung' },
            { id: '3', label: 'Miền Nam' },
      ];
      
      
      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.max(200)] }),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required] }),
            parentId: this.formBuilder.control<string | null>(null),
            managerId: this.formBuilder.control<string | null>(null),
            region: this.formBuilder.control<number>(0, { nonNullable: true })
      })


      onManagerSelected(opt: KitDropdownOption | null) {
            this.form.patchValue({ managerId: opt?.id ?? null });
      }
      onRegionSelected(opt: KitDropdownOption | null) {
            this.form.patchValue({ region: Number(opt?.id ?? null) });
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