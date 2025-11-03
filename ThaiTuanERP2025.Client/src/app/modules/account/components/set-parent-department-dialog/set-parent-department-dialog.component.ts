import { CommonModule } from "@angular/common";
import { Component, inject, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ToastService } from "../../../../shared/components/toast/toast.service";
import { DepartmentFacade } from "../../facades/department.facade";
import { DepartmentOptionStore } from "../../options/department-dropdown-options.option";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { DepartmentService } from "../../services/department.service";
import { firstValueFrom } from "rxjs";

@Component({
      selector: 'set-parent-department-dialog',
      standalone: true,
      imports: [CommonModule, KitSpinnerButtonComponent, KitDropdownComponent, ReactiveFormsModule ],
      templateUrl: './set-parent-department-dialog.component.html',
})
export class SetParentDepartmentDialogComponent {
      private dialogRef = inject(MatDialogRef<SetParentDepartmentDialogComponent>);
      private toastService = inject(ToastService);
      private departmentService = inject(DepartmentService);
      private departmentFacade = inject(DepartmentFacade);
      departments$ = this.departmentFacade.departments$;
      private departmentOptionStore = inject(DepartmentOptionStore);
      departmentOptions$ = this.departmentOptionStore.option$;
      private formBuilder = inject(FormBuilder);
      departmentId: string = '';
      isSubmitting = false;

      form = this.formBuilder.group({
            parentId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ] })
      });

      constructor( @Inject(MAT_DIALOG_DATA) public data: string ) {
            this.departmentId = data;
            this.loadParentDepartment(this.departmentId);
      }

      async loadParentDepartment(departmentId: string) {
            var parentDept = await firstValueFrom(this.departmentService.getParentDepartment(departmentId));
            console.log('Parent Dept:', parentDept);
            if (parentDept) {
                  this.form.patchValue({ parentId: parentDept.id });
            }
      }

      onDepartmentSelected(opt: KitDropdownOption): void {
            this.form.patchValue({ parentId: opt.id });
      }

      async submit() {
            if (this.form.invalid) {
                  this.toastService.error('Vui lòng điền đầy đủ thông tin.');
                  return;
            }

            this.isSubmitting = true;

            try {
                  const parentId = this.form.value.parentId === '' ? null : this.form.value.parentId;
                  await this.departmentFacade.setParent(this.departmentId, parentId!);
                  this.toastService.successRich('Cập nhật thành công.');
                  this.close(this.form.value);
            } catch (error) {
                  this.toastService.error('Cập nhật thất bại.');
            } finally {
                  this.isSubmitting = false;
            }
      }

      close(result?: any) {
            this.dialogRef.close(result);
      }
      
}