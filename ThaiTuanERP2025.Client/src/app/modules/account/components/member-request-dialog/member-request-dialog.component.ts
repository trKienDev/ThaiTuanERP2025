import { CommonModule } from "@angular/common";
import { Component, OnInit, inject, Inject } from "@angular/core";
import { ReactiveFormsModule, FormBuilder, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { firstValueFrom } from "rxjs";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { UserFacade } from "../../facades/user.facade";
import { UserRequest } from "../../models/user.model";
import { DepartmentOptionStore } from "../../options/department-dropdown.option";
import { UserOptionStore } from "../../options/user-dropdown.option";
import { RoleDropdownOptionsStore } from "../../options/role-dropdown.options";
import { KitSpinnerButtonComponent } from "../../../../shared/components/kit-spinner-button/kit-spinner-button.component";

@Component({
      selector: 'member-request-dialog',
      standalone: true,
      imports: [CommonModule, ReactiveFormsModule, KitDropdownComponent, KitSpinnerButtonComponent],
      templateUrl: './member-request-dialog.component.html',
})
export class MemberRequestDialog implements OnInit {
      private readonly dialogRef = inject(MatDialogRef<MemberRequestDialog>);
      private readonly formBuilder = inject(FormBuilder);
      private readonly toastService = inject(ToastService);

      private readonly userFacade = inject(UserFacade);
      private readonly userOptionStore = inject(UserOptionStore);
      managerOptions$ = this.userOptionStore.option$;

      private readonly departmentOptionStore = inject(DepartmentOptionStore);
      departmentOptions$ = this.departmentOptionStore.option$;

      private readonly roleOptionsStore = inject(RoleDropdownOptionsStore);
      roleOptions$ = this.roleOptionsStore.option$;

      constructor(
            @Inject(MAT_DIALOG_DATA) public data?: { user?: UserRequest }
      ) {}

      dialogTitle = 'Thêm người dùng mới';
      submitting = false;

      form = this.formBuilder.group({
            fullName: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.maxLength(100) ]}),
            username: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.minLength(4), Validators.maxLength(100) ]}),
            employeeCode: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.minLength(4), Validators.maxLength(100) ]}),
            email: this.formBuilder.control<string | null>('', {  validators: [ Validators.email, Validators.maxLength(100) ]}),
            password: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.minLength(6), Validators.maxLength(100) ]}),
            roleId: this.formBuilder.control<string | null>(null),
            phone: this.formBuilder.control<number | null>(null),
            departmentId: this.formBuilder.control<string | null>(null),
            position: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.maxLength(100) ]}),
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
                        roleId: u.roleId,
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
            this.form.patchValue({ roleId: opt.id });
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