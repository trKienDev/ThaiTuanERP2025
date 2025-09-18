import { CommonModule } from "@angular/common";
import { Component, inject, OnInit } from "@angular/core";
import { DepartmentService } from "../../../services/department.service";
import { MatDialogRef } from "@angular/material/dialog";
import { UserService } from "../../../services/user.service";
import { KitDropdownOption, KitDropdownComponent } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { resolveAvatarUrl } from "../../../../../shared/utils/avatar.utils";
import { environment } from "../../../../../../environments/environment";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { FormBuilder, Validators } from "@angular/forms";

@Component({
      selector: 'department-request-dialog',
      standalone: true,
      imports: [CommonModule, KitDropdownComponent],
      templateUrl: './department-request.component.html',
})
export class DepartmentRequestDialog implements OnInit {
      private departmentService = inject(DepartmentService);
      private userService = inject(UserService);
      private dialogRef = inject(MatDialogRef<DepartmentRequestDialog>);
      private toastService = inject(ToastService);
      private formBuilder = inject(FormBuilder);
      private baseUrl = environment.baseUrl;

      dialogTitle = 'Thêm phòng ban mới';
      submitting = false;
      managerOptions: KitDropdownOption[] = [];
      
      form = this.formBuilder.group({
            name: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required, Validators.max(200)] }),
            code: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required] }),
            managerId: this.formBuilder.control<string>('', { nonNullable: true, validators: [ Validators.required ]})
      })

      ngOnInit(): void {
            this.loadManagers();
      }

      loadManagers(): void {
            this.userService.getAll().subscribe({
                  next: (users) => {
                        console.log('user: ', users);
                        this.managerOptions = users.map(u => ({
                              id: u.id,
                              label: u.fullName,
                              imgUrl: resolveAvatarUrl(this.baseUrl, u)
                        }));
                  }, 
                  error: (err) => {
                        const messages = handleHttpError(err).join('\n');
                        this.toastService.errorRich(messages || 'Lỗi khi tải danh sách users');
                  }
            })
      }
      onManagerSelected(opt: KitDropdownOption) {
            this.form.patchValue({ managerId: opt.id });
      }

      close(result?: any) {
            this.dialogRef.close();
      }
}