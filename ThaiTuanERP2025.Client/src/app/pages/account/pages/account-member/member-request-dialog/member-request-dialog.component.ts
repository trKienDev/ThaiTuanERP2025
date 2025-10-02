import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { UserOptionStore } from "../../../options/user-dropdown-options.store";
import { DepartmentOptionStore } from "../../../options/department-dropdown-options.option";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { firstValueFrom } from "rxjs";
import { UserFacade } from "../../../facades/user.facade";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { UserRequest } from "../../../models/user.model";

@Component({
      selector: 'member-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent],
      templateUrl: './member-request-dialog.component.html',
})
export class MemberRequestDialog implements OnInit {
      private dialogRef = inject(MatDialogRef<MemberRequestDialog>);
      private formBuilder = inject(FormBuilder);
      private toastService = inject(ToastService);

      private userFacade = inject(UserFacade);
      private userOptionStore = inject(UserOptionStore);
      managerOptions$ = this.userOptionStore.option$;

      private departmentOptionStore = inject(DepartmentOptionStore);
      departmentOptions$ = this.departmentOptionStore.option$;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data?: { user?: UserRequest }
      ) {}

      dialogTitle = 'Thêm người dùng mới';
      submitting = false;

      userRoleOptions: KitDropdownOption[] = [
            { id: '0', label: 'Admin' },
            { id: '1', label: 'Quản lý' },
            { id: '2', label: 'Nhân viên' },
      ]

      form = this.formBuilder.group({
            fullName: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.maxLength(100) ]}),
            username: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.minLength(4), Validators.maxLength(100) ]}),
            employeeCode: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.minLength(4), Validators.maxLength(100) ]}),
            email: this.formBuilder.control<string | null>(null, {validators: [ Validators.email, Validators.maxLength(100) ]}),
            password: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.minLength(6), Validators.maxLength(100) ]}),
            role: this.formBuilder.control<string>('0', { nonNullable: true }),
            phone: this.formBuilder.control<number | null>(null, {}),
            departmentId: this.formBuilder.control<string | null>(null),
            position: this.formBuilder.control<string>('', { nonNullable: true })
      });

      ngOnInit(): void {
            if(this.data?.user) {
                  this.dialogTitle = 'Cập nhật thông tin người dùng';
                  const u = this.data.user;
                  this.form.patchValue({
                        fullName: u.fullName,
                        username: u.username,
                        employeeCode: u.employeeCode,
                        email: u.email,
                        password: u.password,
                        role: u.role,
                        phone: u.phone,
                        departmentId: u.departmentId,
                        position: u.position,
                  })
            }
      }

      onDepartmentSelected(opt: KitDropdownOption | null) {
            this.form.patchValue({ departmentId: opt?.id ?? null });
      }
      onUserRoleSelected(opt: KitDropdownOption) {
            this.form.patchValue({ role: opt.id });
      }

      async submit(): Promise<void> {
            this.form.markAllAsTouched();
            if(this.form.invalid) {
                  alert('error');
                  return;
            }
            
            this.submitting = true;

            try {
                  const payload: UserRequest = this.form.getRawValue();
                  console.log('payload: ', payload);
                  const created = await firstValueFrom(this.userFacade.create(payload));
                  this.toastService.successRich('Thêm người dùng thành công');
                  this.dialogRef.close({ isSuccess: true, response: created });
            } catch(error) {
                  const messages = handleHttpError(error).join('\n');
                  this.toastService.errorRich(messages || 'Lỗi khi thêm người dùng');
            } finally {
                  this.submitting = false;
            }
      }

      close(result?: any) {
            this.dialogRef.close(result);
      }

}