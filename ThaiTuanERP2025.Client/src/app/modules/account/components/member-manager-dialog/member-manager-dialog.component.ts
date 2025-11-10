import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { KitDropdownComponent, KitDropdownOption } from "../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { UserFacade } from "../../facades/user.facade";
import { UserOptionStore } from "../../options/user-dropdown-options.store";
import { SetUserManagerRequest, UserDto } from "../../models/user.model";
import { UserService } from "../../services/user.service";
import { ToastService } from "../../../../shared/components/kit-toast-alert/kit-toast-alert.service";
import { handleHttpError } from "../../../../shared/utils/handle-http-errors.util";
import { firstValueFrom } from "rxjs";
import { HttpErrorResponse } from "@angular/common/http";
import { ConfirmService } from "../../../../shared/components/confirm-dialog/confirm.service";

@Component({
      selector: 'member-manager-dialog',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule ,KitDropdownComponent],
      templateUrl: './member-manager-dialog.component.html'
})
export class MemberManagerDialog implements OnInit {
      private readonly toast = inject(ToastService);
      private readonly dialogRef = inject(MatDialogRef<MemberManagerDialog>);
      private readonly formBuilder = inject(FormBuilder);
      private readonly confirm = inject(ConfirmService);
      user!: UserDto;
      
      private readonly userFacade = inject(UserFacade);
      private readonly userOptionsStore = inject(UserOptionStore);
      managerOptions$ = this.userOptionsStore.option$;
      private readonly userService = inject(UserService);

      constructor(
            @Inject(MAT_DIALOG_DATA) public data: UserDto 
      ) {}

      submitting = false;

      form = this.formBuilder.group({
            managerIds: this.formBuilder.control<string[]>([], { nonNullable: true }),
            primaryManagerId: this.formBuilder.control<string | null>(null)
      })

      ngOnInit(): void {
            if(this.data) {
                  this.user = this.data;
                  this.userService.getManagerIds(this.user.id).subscribe({
                        next: (ids) => {
                              this.form.patchValue({ managerIds:  ids });
                        },
                        error: (err) => {
                              const messages = handleHttpError(err).join('\n');
                              this.toast.errorRich(messages || 'Không thể tải danh sách quản lý');
                        }
                  })
            }
      }

      onManagerSelected(opt: KitDropdownOption) {
            const id = typeof opt === 'string' ? opt : opt.id;
            const ctrl = this.form.controls.managerIds;
            const current = ctrl.getRawValue() ?? [];
            if (!current.includes(id)) ctrl.setValue([...current, id]);
            ctrl.markAsDirty();
            ctrl.updateValueAndValidity();
      }

      async submit(): Promise<void> {
            this.form.markAllAsTouched();
            if(this.form.invalid) {
                  alert('error');
                  return;
            }

            this.submitting = true;

            try {
                  const raw = this.form.getRawValue();
                  const payload: SetUserManagerRequest = {
                        managerIds: raw.managerIds,
                        primaryManagerId: raw.primaryManagerId ?? undefined
                  };
                  const created = await firstValueFrom(this.userService.setManagers(this.user.id, payload));
                  this.toast.successRich('Thêm quản lý thành công');
                  this.dialogRef.close({ isSuccess: true, response: created });
            } catch(error) {
                  if (error instanceof HttpErrorResponse && [401,403,500,0].includes(error.status ?? 0)) {
                        return;
                  }
                  if (error instanceof HttpErrorResponse && error.status === 404) {
                        this.toast?.warningRich('Không tìm thấy dữ liệu để tạo mã ngân sách.');
                  } else if (error instanceof HttpErrorResponse && error.status === 400) {
                        this.toast?.errorRich(error.error?.message || 'Dữ liệu không hợp lệ.');
                  } else {
                        const messages = handleHttpError(error).join('\n');
                        this.confirm.error$(messages);
                        this.toast?.errorRich('Tạo mã ngân sách thất bại.');
                  }
            } finally {
                  this.submitting = false;
            }
      }

      close(): void {
            this.dialogRef.close();
      }
}