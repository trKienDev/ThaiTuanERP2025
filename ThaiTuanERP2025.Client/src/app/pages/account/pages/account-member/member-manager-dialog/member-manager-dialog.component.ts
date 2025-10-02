import { CommonModule } from "@angular/common";
import { Component, Inject, inject, OnInit } from "@angular/core";
import { KitDropdownComponent, KitDropdownOption } from "../../../../../shared/components/kit-dropdown/kit-dropdown.component";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { UserFacade } from "../../../facades/user.facade";
import { UserOptionStore } from "../../../options/user-dropdown-options.store";
import { SetUserManagerRequest, UserDto } from "../../../models/user.model";
import { UserService } from "../../../services/user.service";
import { ToastService } from "../../../../../shared/components/toast/toast.service";
import { handleHttpError } from "../../../../../shared/utils/handle-http-errors.util";
import { firstValueFrom } from "rxjs";

@Component({
      selector: 'member-manager-dialog',
      standalone: true,
      imports: [ CommonModule, ReactiveFormsModule ,KitDropdownComponent],
      templateUrl: './member-manager-dialog.component.html'
})
export class MemberManagerDialog implements OnInit {
      private toastService = inject(ToastService);
      private dialogRef = inject(MatDialogRef<MemberManagerDialog>);
      private formBuilder = inject(FormBuilder);
      user!: UserDto;
      
      private userFacade = inject(UserFacade);
      private userOptionsStore = inject(UserOptionStore);
      managerOptions$ = this.userOptionsStore.option$;
      private userService = inject(UserService);

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
                              this.toastService.errorRich(messages || 'Không thể tải danh sách quản lý');
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
                  this.toastService.successRich('Thêm quản lý thành công');
                  this.dialogRef.close({ isSuccess: true, response: created });
            } catch(error) {
                  const messages = handleHttpError(error).join('\n');
                  this.toastService.errorRich(messages || 'Lỗi khi thêm quản lý');
            } finally {
                  this.submitting = false;
            }
      }

      close(): void {
            this.dialogRef.close();
      }
}