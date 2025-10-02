import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { UserOptionStore } from "../../../options/user-dropdown-options.store";
import { DepartmentDto, SetDepartmentManagerRequest } from "../../../models/department.model";
import { firstValueFrom } from "rxjs";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { DepartmentFacade } from "../../../facades/department.facade";

@Component({
      selector: 'department-manager-dialog',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule, KitDropdownComponent ],
      templateUrl: './department-manager-dialog.component.html'
})
export class DepartmentManagerDialogComponent implements OnInit {
      private toastService = inject(ToastService);
      private dialogRef = inject(MatDialogRef<DepartmentManagerDialogComponent>);
      private formBuilder = inject(FormBuilder);
      private readonly departmentFacade = inject(DepartmentFacade);

      depatment!: DepartmentDto;

      private userOptionStore = inject(UserOptionStore);
      managerOptions$ = this.userOptionStore.option$;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: DepartmentDto 
      ) {}

      submitting = false;

      form = this.formBuilder.group({
            managerId: this.formBuilder.control<string>('', { nonNullable: true }),
      })

      ngOnInit(): void {
            console.log('data: ', this.data);
            if(this.data) {
                  this.depatment = this.data;
                  if(this.depatment.managerUserId) {
                        this.form.patchValue({ managerId: this.depatment.managerUserId });
                  }
            }
      }

      onManagerSelected(opt: KitDropdownOption) {
            this.form.patchValue({ managerId: opt.id });
      }

      async submit(): Promise<void> {
            this.form.markAllAsTouched();
            if(this.form.invalid) {
                  alert('error');
                  return;
            }

            this.submitting = true
            try {
                  const raw = this.form.getRawValue();
                  const payload: SetDepartmentManagerRequest = {
                        managerId: raw.managerId,
                        departmentId: this.depatment.id
                  };
                  const result = await firstValueFrom(this.departmentFacade.setManager(this.depatment.id, payload));
                  this.toastService.successRich("Thiết lập quản lý thành công");
                  this.dialogRef.close({ isSuccess: true, response: result });
            } catch(error) {
                  const messages = handleHttpError(error).join('\n');
                  this.toastService.errorRich(messages || 'Lỗi khi thiết lập quản lý');
            } finally {
                  this.submitting = false;
            }
      }

      close(): void {
            this.dialogRef.close();
      }
}